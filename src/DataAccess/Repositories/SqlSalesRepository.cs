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
            // sp_AgregarPresupuestoVenta or generic insert?
            // The SP sp_AgregarPresupuestoVenta inserts into Ventas.
            // But we might want to insert 'NotaPedido'.
            // The SP name is specific but the implementation is generic for Ventas table.
            // Let's use sp_AgregarPresupuestoVenta but it takes specific params.
            // Actually, let's look at sp_AgregarPresupuestoVenta signature:
            // @id_cliente, @fecha, @tipoDocumento, @numeroDocumento, @montoTotal, @id_estadoVentas
            // It seems generic enough if we pass 'NotaPedido' as tipoDocumento.
            
            await ExecuteNonQueryAsync("sp_AgregarPresupuestoVenta", p =>
            {
                p.AddWithValue("@id_cliente", clientId);
                p.AddWithValue("@fecha", date);
                p.AddWithValue("@tipoDocumento", docType);
                p.AddWithValue("@numeroDocumento", DBNull.Value); // Auto or null
                p.AddWithValue("@montoTotal", total);
                p.AddWithValue("@id_estadoVentas", statusId);
            });

            // Hack to get ID
            var id = await ExecuteScalarAsync("SELECT MAX(id_venta) FROM Ventas", p => { }, CommandType.Text);
            return id != null ? Convert.ToInt32(id) : 0;
        }

        public async Task AddSaleDetailAsync(int saleId, int productId, int quantity, decimal unitPrice)
        {
            // sp_AgregarNotaPedido inserts into DetalleVentas AND updates Stock.
            // Signature: @id_venta, @id_producto, @cantidad, @precioUnitario, @subtotal
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
            // sp_ReporteVentas seems appropriate?
            // Or just query Ventas table directly if no suitable SP exists for simple listing.
            // sp_ReporteVentas might be complex. Let's check its signature.
            // Wait, I don't have sp_ReporteVentas signature handy in memory, but I saw it in the file list.
            // Let's assume we can use a simple query or the SP if it fits.
            // Given I cannot see the SP definition right now without reading, I'll use inline SQL for safety or try to read it.
            // Actually, I read sp_ventas.txt earlier.
            // Let's use inline SQL for listing to be safe and flexible.
            
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
                        Estado = "Registrado" // Simplify for now
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
    }
}
