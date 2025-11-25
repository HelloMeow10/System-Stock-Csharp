using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SharedKernel;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlSalesRepository : ISalesRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlSalesRepository> _logger;

        public SqlSalesRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlSalesRepository> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<T> ExecuteReaderAsync<T>(string sql, Func<SqlDataReader, Task<T>> map, Action<SqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (var connection = (SqlConnection)_connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = commandType;
                        addParameters?.Invoke(command.Parameters);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            return await map(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL Error executing {sql}", sql);
                throw new DataAccessLayerException($"SQL Error executing {sql}", ex);
            }
        }

        private async Task ExecuteNonQueryAsync(string sql, Action<SqlParameterCollection> addParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (var connection = (SqlConnection)_connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = commandType;
                        addParameters(command.Parameters);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL Error executing {sql}", sql);
                throw new DataAccessLayerException($"SQL Error executing {sql}", ex);
            }
        }

        private async Task<object?> ExecuteScalarAsync(string sql, Action<SqlParameterCollection> addParameters, CommandType commandType = CommandType.StoredProcedure)
        {
             try
            {
                using (var connection = (SqlConnection)_connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = commandType;
                        addParameters(command.Parameters);
                        return await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL Error executing {sql}", sql);
                throw new DataAccessLayerException($"SQL Error executing {sql}", ex);
            }
        }

        public async Task<int> CreateSaleAsync(int clientId, DateTime date, string docType, decimal total, int statusId)
        {
            await ExecuteNonQueryAsync("sp_AgregarPresupuestoVenta", p =>
            {
                p.AddWithValue("@id_cliente", clientId);
                p.AddWithValue("@fecha", date);
                p.AddWithValue("@tipoDocumento", docType);
                p.AddWithValue("@numeroDocumento", DBNull.Value); 
                p.AddWithValue("@montoTotal", total);
                p.AddWithValue("@id_estadoVentas", statusId);
            });

            var id = await ExecuteScalarAsync("SELECT MAX(id_venta) FROM Ventas", p => { }, CommandType.Text);
            return id != null ? Convert.ToInt32(id) : 0;
        }

        public async Task AddSaleDetailAsync(int saleId, int productId, int quantity, decimal unitPrice)
        {
            await ExecuteNonQueryAsync("sp_AgregarNotaPedido", p =>
            {
                p.AddWithValue("@id_venta", saleId);
                p.AddWithValue("@id_producto", productId);
                p.AddWithValue("@cantidad", quantity);
                p.AddWithValue("@precioUnitario", unitPrice);
                p.AddWithValue("@subtotal", quantity * unitPrice);
            });
        }

        public async Task<IEnumerable<SaleOrderDto>> GetSalesAsync(DateTime? start, DateTime? end, int? clientId)
        {
            string sql = @"
                SELECT v.id_venta, c.nombre as Cliente, v.fecha, v.tipoDocumento, v.montoTotal, ev.facturada 
                FROM Ventas v
                INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
                LEFT JOIN EstadoVentas ev ON v.id_estadoVentas = ev.id_estadoVentas
                WHERE (@start IS NULL OR v.fecha >= @start)
                  AND (@end IS NULL OR v.fecha <= @end)
                  AND (@clientId IS NULL OR v.id_cliente = @clientId)";

            return await ExecuteReaderAsync(sql, async reader =>
            {
                var list = new List<SaleOrderDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(new SaleOrderDto
                    {
                        Id = (int)reader["id_venta"],
                        Cliente = reader["Cliente"] as string ?? "",
                        Fecha = (DateTime)reader["fecha"],
                        TipoDocumento = reader["tipoDocumento"] as string ?? "",
                        Total = (decimal)reader["montoTotal"],
                        Estado = "Registrado" 
                    });
                }
                return list;
            }, p =>
            {
                p.AddWithValue("@start", (object?)start ?? DBNull.Value);
                p.AddWithValue("@end", (object?)end ?? DBNull.Value);
                p.AddWithValue("@clientId", (object?)clientId ?? DBNull.Value);
            }, CommandType.Text);
        }

        public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? start, DateTime? end)
        {
            return await ExecuteReaderAsync("sp_ReporteVentas", async reader =>
            {
                var list = new List<SalesReportDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(new SalesReportDto(
                        IdVenta: (int)reader["id_venta"],
                        Fecha: (DateTime)reader["fecha"],
                        Cliente: reader["Cliente"] as string ?? "",
                        Producto: reader["Producto"] as string ?? "",
                        Categoria: reader["Categoria"] as string ?? "",
                        Cantidad: (int)reader["cantidad"],
                        PrecioUnitario: (decimal)reader["precioUnitario"],
                        Subtotal: (decimal)reader["subtotal"],
                        MontoTotalVenta: (decimal)reader["montoTotal"]
                    ));
                }
                return list;
            }, p =>
            {
                p.AddWithValue("@fechaDesde", (object?)start ?? DBNull.Value);
                p.AddWithValue("@fechaHasta", (object?)end ?? DBNull.Value);
            }, CommandType.StoredProcedure);
        }
    }
}
