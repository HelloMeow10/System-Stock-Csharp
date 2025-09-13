using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SharedKernel;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlUserRepository> _logger;

        public SqlUserRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlUserRepository> logger)
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
                _logger.LogError(ex, "Ocurrió un error de SQL al ejecutar ExecuteReaderAsync para el comando: {sql}", sql);
                throw new DataAccessLayerException($"Ocurrió un error de SQL al ejecutar {sql}", ex);
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
                _logger.LogError(ex, "Ocurrió un error de SQL al ejecutar ExecuteNonQueryAsync para el comando: {sql}", sql);
                throw new DataAccessLayerException($"Ocurrió un error de SQL al ejecutar {sql}", ex);
            }
        }

        public async Task<List<HistorialContrasena>> GetHistorialContrasenasByUsuarioIdAsync(int idUsuario) => await ExecuteReaderAsync("sp_get_historial_contrasenas_by_usuario_id", async reader =>
        {
            var list = new List<HistorialContrasena>();
            while (await reader.ReadAsync())
            {
                list.Add(new HistorialContrasena
                {
                    IdHistorial = (int)reader["id"],
                    IdUsuario = (int)reader["id_usuario"],
                    FechaCambio = (DateTime)reader["fecha_cambio"],
                    ContrasenaScript = (byte[])reader["contrasena_script"]
                });
            }
            return list;
        }, p => p.AddWithValue("@id_usuario", idUsuario), CommandType.StoredProcedure);

        public async Task<Usuario?> GetUsuarioByNombreUsuarioAsync(string nombre) => await ExecuteReaderAsync("sp_get_usuario_by_nombre", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return MapToUsuario(reader);
        }, p => p.AddWithValue("@usuario_nombre", nombre), CommandType.StoredProcedure);

        public async Task Set2faCodeAsync(string username, string? code, DateTime? expiry) => await ExecuteNonQueryAsync(
            "sp_set_2fa_code",
            p =>
            {
                p.AddWithValue("@code", (object?)code ?? DBNull.Value);
                p.AddWithValue("@expiry", (object?)expiry ?? DBNull.Value);
                p.AddWithValue("@username", username);
            },
            CommandType.StoredProcedure
        );

        public async Task<PagedList<Usuario>> GetUsersAsync(PaginationParams paginationParams)
        {
            // This is a temporary in-memory pagination implementation.
            // For production, this should be replaced with a stored procedure
            // that performs pagination at the database level for efficiency.
            var allUsers = await GetAllUsersAsync();
            return PagedList<Usuario>.ToPagedList(allUsers, paginationParams.PageNumber, paginationParams.PageSize);
        }

        private async Task<List<Usuario>> GetAllUsersAsync() => await ExecuteReaderAsync("sp_get_all_users", async reader =>
        {
            var list = new List<Usuario>();
            while (await reader.ReadAsync())
            {
                list.Add(MapToUsuario(reader));
            }
            return list;
        }, commandType: CommandType.StoredProcedure);

        private static Usuario MapToUsuario(SqlDataReader reader)
        {
            return Usuario.FromDataReader(reader);
        }

        public async Task AddHistorialContrasenaAsync(HistorialContrasena historial) => await ExecuteNonQueryAsync("sp_historial_contrasena", p =>
        {
            p.AddWithValue("@id_usuario", historial.IdUsuario);
            p.AddWithValue("@contrasena_script", historial.ContrasenaScript);
        });

        public async Task AddUsuarioAsync(Usuario usuario) => await ExecuteNonQueryAsync("sp_insert_usuario", p =>
        {
            p.AddWithValue("@usuario", usuario.UsuarioNombre);
            p.AddWithValue("@contrasena_script", usuario.ContrasenaScript);
            p.AddWithValue("@id_persona", usuario.IdPersona);
            p.AddWithValue("@fecha_bloqueo", usuario.FechaBloqueo);
            p.AddWithValue("@nombre_usuario_bloqueo", (object?)usuario.NombreUsuarioBloqueo ?? DBNull.Value);
            p.AddWithValue("@fecha_ultimo_cambio", usuario.FechaUltimoCambio);
            p.AddWithValue("@id_rol", usuario.IdRol);
            p.AddWithValue("@CambioContrasenaObligatorio", usuario.CambioContrasenaObligatorio);
        });

        public async Task UpdateUsuarioAsync(Usuario usuario) => await ExecuteNonQueryAsync("sp_actualizar_usuario", p =>
        {
            p.AddWithValue("@id_usuario", usuario.IdUsuario);
            p.AddWithValue("@usuario", usuario.UsuarioNombre);
            p.AddWithValue("@contrasena_script", usuario.ContrasenaScript);
            p.AddWithValue("@id_persona", usuario.IdPersona);
            p.AddWithValue("@fecha_bloqueo", usuario.FechaBloqueo);
            p.AddWithValue("@nombre_usuario_bloqueo", (object?)usuario.NombreUsuarioBloqueo ?? DBNull.Value);
            p.AddWithValue("@fecha_ultimo_cambio", usuario.FechaUltimoCambio);
            p.AddWithValue("@id_rol", usuario.IdRol);
            p.AddWithValue("@CambioContrasenaObligatorio", usuario.CambioContrasenaObligatorio);
        });

        public async Task DeleteUsuarioAsync(int usuarioId) => await ExecuteNonQueryAsync("sp_delete_usuario", p =>
        {
            p.AddWithValue("@id_usuario", usuarioId);
        });

        public async Task<Usuario?> GetUsuarioByIdAsync(int id) => await ExecuteReaderAsync("sp_get_usuario_by_id", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return MapToUsuario(reader);
        }, p => p.AddWithValue("@id_usuario", id), CommandType.StoredProcedure);
    }
}
