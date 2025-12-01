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
            return await ExecuteReaderAsync("sp_GetAllProveedoresExtended", async reader =>
            {
                var list = new List<SupplierDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(MapExtended(reader));
                }
                return list;
            }, null, CommandType.StoredProcedure);
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            var supplier = await ExecuteReaderAsync("sp_GetProveedorByIdExtended", async reader =>
            {
                if (await reader.ReadAsync()) return MapExtended(reader);
                return null;
            }, p => p.AddWithValue("@id", id), CommandType.StoredProcedure);
            if (supplier != null)
            {
                supplier.Contactos = await GetContactosAsync(supplier.Codigo);
            }
            return supplier;
        }

        public async Task<IEnumerable<SupplierDto>> SearchByNameAsync(string name)
        {
            return await ExecuteReaderAsync("sp_SearchProveedoresByNombreExtended", async reader =>
            {
                var list = new List<SupplierDto>();
                while (await reader.ReadAsync()) list.Add(MapExtended(reader));
                return list;
            }, p => p.AddWithValue("@nombre", name), CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SupplierDto>> SearchByCuitAsync(string cuit)
        {
            return await ExecuteReaderAsync("sp_SearchProveedoresByCUITExtended", async reader =>
            {
                var list = new List<SupplierDto>();
                while (await reader.ReadAsync()) list.Add(MapExtended(reader));
                return list;
            }, p => p.AddWithValue("@cuit", cuit), CommandType.StoredProcedure);
        }

        public async Task AddAsync(CreateSupplierRequest supplier)
        {
            await ExecuteNonQueryAsync("sp_AgregarProveedorExtended", p =>
            {
                p.AddWithValue("@codigo", supplier.Codigo);
                p.AddWithValue("@nombre", supplier.Nombre);
                p.AddWithValue("@razonSocial", supplier.RazonSocial);
                p.AddWithValue("@CUIT", supplier.Cuit);
                p.AddWithValue("@TiempoEntrega", supplier.TiempoEntrega);
                p.AddWithValue("@Descuento", supplier.Descuento);
                p.AddWithValue("@id_formaPago", supplier.IdFormaPago);
                p.AddWithValue("@Email", supplier.Email);
                p.AddWithValue("@Telefono", supplier.Telefono);
                p.AddWithValue("@Direccion", supplier.Direccion);
                p.AddWithValue("@Provincia", supplier.Provincia);
                p.AddWithValue("@Ciudad", supplier.Ciudad);
                p.AddWithValue("@CondicionIVA", supplier.CondicionIva);
                p.AddWithValue("@PlazoPagoDias", supplier.PlazoPagoDias);
                p.AddWithValue("@Observaciones", supplier.Observaciones);
            });
            // Contacts insertion (optional, ignore if empty)
            foreach (var c in supplier.Contactos)
            {
                await ExecuteNonQueryAsync("sp_AgregarProveedorContacto", p =>
                {
                    p.AddWithValue("@codigo", supplier.Codigo);
                    p.AddWithValue("@Nombre", c.Nombre);
                    p.AddWithValue("@Cargo", c.Cargo);
                    p.AddWithValue("@Email", c.Email);
                    p.AddWithValue("@Telefono", c.Telefono);
                });
            }
        }

        public async Task UpdateAsync(UpdateSupplierRequest supplier)
        {
            await ExecuteNonQueryAsync("sp_ModificarProveedorExtended", p =>
            {
                p.AddWithValue("@id_proveedor", supplier.Id);
                p.AddWithValue("@codigo", supplier.Codigo);
                p.AddWithValue("@nombre", supplier.Nombre);
                p.AddWithValue("@razonSocial", supplier.RazonSocial);
                p.AddWithValue("@CUIT", supplier.Cuit);
                p.AddWithValue("@TiempoEntrega", supplier.TiempoEntrega);
                p.AddWithValue("@Descuento", supplier.Descuento);
                p.AddWithValue("@id_formaPago", supplier.IdFormaPago);
                p.AddWithValue("@Email", supplier.Email);
                p.AddWithValue("@Telefono", supplier.Telefono);
                p.AddWithValue("@Direccion", supplier.Direccion);
                p.AddWithValue("@Provincia", supplier.Provincia);
                p.AddWithValue("@Ciudad", supplier.Ciudad);
                p.AddWithValue("@CondicionIVA", supplier.CondicionIva);
                p.AddWithValue("@PlazoPagoDias", supplier.PlazoPagoDias);
                p.AddWithValue("@Observaciones", supplier.Observaciones);
            });
            // Replace contacts simplistic: delete all then re-insert
            await ExecuteNonQueryAsync("sp_DeleteProveedorContactos", p =>
            {
                p.AddWithValue("@codigo", supplier.Codigo);
            });
            foreach (var c in supplier.Contactos)
            {
                await ExecuteNonQueryAsync("sp_AgregarProveedorContacto", p =>
                {
                    p.AddWithValue("@codigo", supplier.Codigo);
                    p.AddWithValue("@Nombre", c.Nombre);
                    p.AddWithValue("@Cargo", c.Cargo);
                    p.AddWithValue("@Email", c.Email);
                    p.AddWithValue("@Telefono", c.Telefono);
                });
            }
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteNonQueryAsync("DELETE FROM Proveedores WHERE id_proveedor = @id_proveedor", p =>
            {
                p.AddWithValue("@id_proveedor", id);
            }, CommandType.Text);
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

        private static SupplierDto MapExtended(SqlDataReader reader)
        {
            var dto = new SupplierDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                Codigo = reader["codigo"] as string ?? "",
                Nombre = reader["nombre"] as string ?? "",
                RazonSocial = reader["razonSocial"] as string ?? "",
                Cuit = reader["CUIT"] as string ?? "",
                TiempoEntrega = reader["TiempoEntrega"] != DBNull.Value ? Convert.ToInt32(reader["TiempoEntrega"]) : 0,
                Descuento = reader["Descuento"] != DBNull.Value ? Convert.ToDecimal(reader["Descuento"]) : 0m,
                IdFormaPago = reader["id_formaPago"] != DBNull.Value ? Convert.ToInt32(reader["id_formaPago"]) : 0,
                Email = reader["Email"] as string ?? "",
                Telefono = reader["Telefono"] as string ?? "",
                Direccion = reader["Direccion"] as string ?? "",
                Provincia = reader["Provincia"] as string ?? "",
                Ciudad = reader["Ciudad"] as string ?? "",
                CondicionIva = reader["CondicionIVA"] as string ?? "",
                PlazoPagoDias = reader["PlazoPagoDias"] != DBNull.Value ? Convert.ToInt32(reader["PlazoPagoDias"]) : 0,
                Observaciones = reader["Observaciones"] as string ?? ""
            };
            return dto;
        }

        private async Task<List<SupplierContactDto>> GetContactosAsync(string codigo)
        {
            return await ExecuteReaderAsync("sp_GetProveedorContactos", async reader =>
            {
                var list = new List<SupplierContactDto>();
                while (await reader.ReadAsync())
                {
                    var contact = new SupplierContactDto
                    {
                        Id = reader["id_contactoProveedor"] != DBNull.Value ? Convert.ToInt32(reader["id_contactoProveedor"]) : null,
                        Nombre = reader["Nombre"] as string ?? string.Empty,
                        Cargo = reader["Cargo"] as string ?? string.Empty,
                        Email = reader["Email"] as string ?? string.Empty,
                        Telefono = reader["Telefono"] as string ?? string.Empty
                    };
                    list.Add(contact);
                }
                return list;
            }, p => p.AddWithValue("@codigo", codigo), CommandType.StoredProcedure);
        }
    }
}
