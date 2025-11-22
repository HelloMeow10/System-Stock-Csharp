using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SharedKernel;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlClientRepository : IClientRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlClientRepository> _logger;

        public SqlClientRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlClientRepository> logger)
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

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            return await SearchByNameAsync("");
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            return await ExecuteReaderAsync("SELECT * FROM Clientes WHERE id_cliente = @id", async reader =>
            {
                if (await reader.ReadAsync())
                {
                    return Map(reader);
                }
                return null;
            }, p => p.AddWithValue("@id", id), CommandType.Text);
        }

        public async Task<IEnumerable<ClientDto>> SearchByNameAsync(string name)
        {
            return await ExecuteReaderAsync("sp_ConsultarClientePorNombre", async reader =>
            {
                var list = new List<ClientDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, p => p.AddWithValue("@nombre", name), CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ClientDto>> SearchByCuitAsync(string cuit)
        {
            return await ExecuteReaderAsync("sp_ConsultarClientePorCUIT", async reader =>
            {
                var list = new List<ClientDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, p => p.AddWithValue("@cuitDni", cuit), CommandType.StoredProcedure);
        }

        public async Task AddAsync(CreateClientRequest client)
        {
            await ExecuteNonQueryAsync("sp_AgregarCliente", p =>
            {
                p.AddWithValue("@codigo", client.Codigo);
                p.AddWithValue("@nombre", client.Nombre);
                p.AddWithValue("@razonSocial", client.RazonSocial);
                p.AddWithValue("@cuitDni", client.CuitDni);
                p.AddWithValue("@id_formaPago", client.IdFormaPago);
                p.AddWithValue("@limiteCredito", client.LimiteCredito);
                p.AddWithValue("@descuento", client.Descuento);
                p.AddWithValue("@estado", client.Estado);
            });
        }

        public async Task UpdateAsync(UpdateClientRequest client)
        {
            await ExecuteNonQueryAsync("sp_ModificarCliente", p =>
            {
                p.AddWithValue("@id_cliente", client.Id);
                p.AddWithValue("@codigo", client.Codigo);
                p.AddWithValue("@nombre", client.Nombre);
                p.AddWithValue("@razonSocial", client.RazonSocial);
                p.AddWithValue("@cuitDni", client.CuitDni);
                p.AddWithValue("@id_formaPago", client.IdFormaPago);
                p.AddWithValue("@limiteCredito", client.LimiteCredito);
                p.AddWithValue("@descuento", client.Descuento);
                p.AddWithValue("@estado", client.Estado);
            });
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteNonQueryAsync("sp_EliminarCliente", p =>
            {
                p.AddWithValue("@id_cliente", id);
            });
        }

        public async Task AddContactAsync(int clientId, string phone, string sector, string schedule, string email)
        {
            await ExecuteNonQueryAsync("sp_AgregarTelefonoCliente", p =>
            {
                p.AddWithValue("@id_cliente", clientId);
                p.AddWithValue("@telefono", phone);
                p.AddWithValue("@sector", sector);
                p.AddWithValue("@horario", schedule);
                p.AddWithValue("@email", email);
            });
        }

        public async Task AddAddressAsync(int clientId, string address, string city, string province, string type)
        {
            await ExecuteNonQueryAsync("sp_AgregarDireccionCliente", p =>
            {
                p.AddWithValue("@id_cliente", clientId);
                p.AddWithValue("@direccion", address);
                p.AddWithValue("@localidad", city);
                p.AddWithValue("@provincia", province);
                p.AddWithValue("@tipo", type);
            });
        }

        private static ClientDto Map(SqlDataReader reader)
        {
            return new ClientDto
            {
                Id = (int)reader["id_cliente"],
                Codigo = reader["codigo"] as string ?? "",
                Nombre = reader["nombre"] as string ?? "",
                RazonSocial = reader["razonSocial"] as string ?? "",
                CuitDni = reader["CUIT_DNI"] as string ?? "",
                IdFormaPago = reader["id_formaPago"] != DBNull.Value ? (int)reader["id_formaPago"] : 0,
                LimiteCredito = reader["limiteCredito"] != DBNull.Value ? (decimal)reader["limiteCredito"] : 0,
                Descuento = reader["descuento"] != DBNull.Value ? (decimal)reader["descuento"] : 0,
                Estado = reader["estado"] as string ?? ""
            };
        }
    }
}
