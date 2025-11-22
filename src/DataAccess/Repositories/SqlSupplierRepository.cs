using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SharedKernel;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlSupplierRepository : ISupplierRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlSupplierRepository> _logger;

        public SqlSupplierRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlSupplierRepository> logger)
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

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            // No SP for GetAll, using inline SQL or creating one. 
            // Using inline SQL for now as per requirement "use SPs" usually implies for modifications, but for queries it's mixed.
            // Actually, let's use sp_ConsultarProveedoresPorNombre with empty string if possible, or just SELECT.
            // sp_ConsultarProveedoresPorNombre uses LIKE '%...%', so empty string matches all.
            return await SearchByNameAsync("");
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            // No SP for GetById? Using inline SQL.
            return await ExecuteReaderAsync("SELECT * FROM Proveedores WHERE id_proveedor = @id", async reader =>
            {
                if (await reader.ReadAsync())
                {
                    return Map(reader);
                }
                return null;
            }, p => p.AddWithValue("@id", id), CommandType.Text);
        }

        public async Task<IEnumerable<SupplierDto>> SearchByNameAsync(string name)
        {
            return await ExecuteReaderAsync("sp_ConsultarProveedoresPorNombre", async reader =>
            {
                var list = new List<SupplierDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, p => p.AddWithValue("@nombre", name), CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SupplierDto>> SearchByCuitAsync(string cuit)
        {
            return await ExecuteReaderAsync("sp_ConsultarProveedoresPorCUIT", async reader =>
            {
                var list = new List<SupplierDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, p => p.AddWithValue("@cuit", cuit), CommandType.StoredProcedure);
        }

        public async Task AddAsync(CreateSupplierRequest supplier)
        {
            await ExecuteNonQueryAsync("sp_AgregarProveedor", p =>
            {
                p.AddWithValue("@codigo", supplier.Codigo);
                p.AddWithValue("@nombre", supplier.Nombre);
                p.AddWithValue("@razonSocial", supplier.RazonSocial);
                p.AddWithValue("@CUIT", supplier.Cuit);
                p.AddWithValue("@TiempoEntrega", supplier.TiempoEntrega);
                p.AddWithValue("@Descuento", supplier.Descuento);
                p.AddWithValue("@id_formaPago", supplier.IdFormaPago);
            });
        }

        public async Task UpdateAsync(UpdateSupplierRequest supplier)
        {
            await ExecuteNonQueryAsync("sp_ModificarProveedor", p =>
            {
                p.AddWithValue("@id_proveedor", supplier.Id);
                p.AddWithValue("@codigo", supplier.Codigo);
                p.AddWithValue("@nombre", supplier.Nombre);
                p.AddWithValue("@razonSocial", supplier.RazonSocial);
                p.AddWithValue("@CUIT", supplier.Cuit);
                p.AddWithValue("@TiempoEntrega", supplier.TiempoEntrega);
                p.AddWithValue("@Descuento", supplier.Descuento);
                p.AddWithValue("@id_formaPago", supplier.IdFormaPago);
            });
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteNonQueryAsync("sp_EliminarProveedor", p =>
            {
                p.AddWithValue("@id_proveedor", id);
            });
        }

        public async Task AddPhoneAsync(int supplierId, string phone, string sector, string schedule, string email)
        {
            await ExecuteNonQueryAsync("sp_AgregarTelefonoProveedor", p =>
            {
                p.AddWithValue("@id_proveedor", supplierId);
                p.AddWithValue("@telefono", phone);
                p.AddWithValue("@sector", sector);
                p.AddWithValue("@horario", schedule);
                p.AddWithValue("@email", email);
            });
        }

        public async Task AddAddressAsync(int supplierId, string address, string city, string province, string type)
        {
            await ExecuteNonQueryAsync("sp_AgregarDireccionProveedor", p =>
            {
                p.AddWithValue("@id_proveedor", supplierId);
                p.AddWithValue("@direccion", address);
                p.AddWithValue("@localidad", city);
                p.AddWithValue("@provincia", province);
                p.AddWithValue("@tipo", type);
            });
        }

        private static SupplierDto Map(SqlDataReader reader)
        {
            return new SupplierDto
            {
                Id = (int)reader["id_proveedor"],
                Codigo = reader["codigo"] as string ?? "",
                Nombre = reader["nombre"] as string ?? "",
                RazonSocial = reader["razonSocial"] as string ?? "",
                Cuit = reader["CUIT"] as string ?? "",
                TiempoEntrega = reader["TiempoEntrega"] != DBNull.Value ? (int)reader["TiempoEntrega"] : 0,
                Descuento = reader["Descuento"] != DBNull.Value ? (decimal)reader["Descuento"] : 0,
                IdFormaPago = reader["id_formaPago"] != DBNull.Value ? (int)reader["id_formaPago"] : 0
            };
        }
    }
}
