using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class SqlReferenceDataRepository : IReferenceDataRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlReferenceDataRepository> _logger;

        public SqlReferenceDataRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlReferenceDataRepository> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<T> ExecuteReaderAsync<T>(string sql, Func<SqlDataReader, Task<T>> map, Action<SqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
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

        public Task<List<TipoDoc>> GetAllTiposDocAsync() => ExecuteReaderAsync("SELECT id_tipo_doc, tipo_doc FROM tipo_doc;", async reader =>
        {
            var list = new List<TipoDoc>();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = (string)reader["tipo_doc"] });
            }
            return list;
        });

        public Task<List<Genero>> GetAllGenerosAsync() => ExecuteReaderAsync("SELECT id_genero, genero FROM generos;", async reader =>
        {
            var list = new List<Genero>();
            while (await reader.ReadAsync())
            {
                list.Add(new Genero { IdGenero = (int)reader["id_genero"], Nombre = (string)reader["genero"] });
            }
            return list;
        });

        public Task<List<Rol>> GetAllRolesAsync() => ExecuteReaderAsync("SELECT id_rol, rol FROM roles;", async reader =>
        {
            var list = new List<Rol>();
            while (await reader.ReadAsync())
            {
                list.Add(new Rol { IdRol = (int)reader["id_rol"], Nombre = (string)reader["rol"] });
            }
            return list;
        });

        public Task<List<Provincia>> GetAllProvinciasAsync() => ExecuteReaderAsync("SELECT id_provincia, provincia FROM provincias;", async reader =>
        {
            var list = new List<Provincia>();
            while (await reader.ReadAsync())
            {
                list.Add(new Provincia { IdProvincia = (int)reader["id_provincia"], Nombre = (string)reader["provincia"] });
            }
            return list;
        });

        public Task<List<Partido>> GetPartidosByProvinciaIdAsync(int provinciaId) => ExecuteReaderAsync("SELECT id_partido, partido, id_provincia FROM partidos WHERE id_provincia = @id_provincia;", async reader =>
        {
            var list = new List<Partido>();
            while (await reader.ReadAsync())
            {
                list.Add(new Partido { IdPartido = (int)reader["id_partido"], Nombre = (string)reader["partido"], IdProvincia = (int)reader["id_provincia"] });
            }
            return list;
        }, p => p.AddWithValue("@id_provincia", provinciaId));

        public Task<List<Localidad>> GetLocalidadesByPartidoIdAsync(int partidoId) => ExecuteReaderAsync("SELECT id_localidad, localidad, id_partido FROM localidades WHERE id_partido = @id_partido;", async reader =>
        {
            var list = new List<Localidad>();
            while (await reader.ReadAsync())
            {
                list.Add(new Localidad { IdLocalidad = (int)reader["id_localidad"], Nombre = (string)reader["localidad"], IdPartido = (int)reader["id_partido"] });
            }
            return list;
        }, p => p.AddWithValue("@id_partido", partidoId));

        public Task<TipoDoc?> GetTipoDocByNombreAsync(string nombre) => ExecuteReaderAsync("SELECT id_tipo_doc, tipo_doc FROM tipo_doc WHERE tipo_doc = @nombre;", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = (string)reader["tipo_doc"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Task<Localidad?> GetLocalidadByNombreAsync(string nombre) => ExecuteReaderAsync("SELECT id_localidad, localidad, id_partido FROM localidades WHERE localidad = @nombre;", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return new Localidad { IdLocalidad = (int)reader["id_localidad"], Nombre = (string)reader["localidad"], IdPartido = (int)reader["id_partido"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Task<Genero?> GetGeneroByNombreAsync(string nombre) => ExecuteReaderAsync("SELECT id_genero, genero FROM generos WHERE genero = @nombre;", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return new Genero { IdGenero = (int)reader["id_genero"], Nombre = (string)reader["genero"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Task<Rol?> GetRolByNombreAsync(string nombre) => ExecuteReaderAsync("SELECT id_rol, rol FROM roles WHERE rol = @nombre;", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return new Rol { IdRol = (int)reader["id_rol"], Nombre = (string)reader["rol"] };
        }, p => p.AddWithValue("@nombre", nombre));
    }
}