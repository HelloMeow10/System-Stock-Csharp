using System;
using System.Collections.Generic;
using System.Data;
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

        private T ExecuteReader<T>(string sql, Func<SqlDataReader, T> map, Action<SqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = (SqlConnection)_connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    addParameters?.Invoke(command.Parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        return map(reader);
                    }
                }
            }
        }

        public List<TipoDoc> GetAllTiposDoc() => ExecuteReader("SELECT id_tipo_doc, tipo_doc FROM tipo_doc;", reader =>
        {
            var list = new List<TipoDoc>();
            while (reader.Read())
            {
                list.Add(new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = (string)reader["tipo_doc"] });
            }
            return list;
        });

        public List<Genero> GetAllGeneros() => ExecuteReader("SELECT id_genero, genero FROM generos;", reader =>
        {
            var list = new List<Genero>();
            while (reader.Read())
            {
                list.Add(new Genero { IdGenero = (int)reader["id_genero"], Nombre = (string)reader["genero"] });
            }
            return list;
        });

        public List<Rol> GetAllRoles() => ExecuteReader("SELECT id_rol, rol FROM roles;", reader =>
        {
            var list = new List<Rol>();
            while (reader.Read())
            {
                list.Add(new Rol { IdRol = (int)reader["id_rol"], Nombre = (string)reader["rol"] });
            }
            return list;
        });

        public List<Provincia> GetAllProvincias() => ExecuteReader("SELECT id_provincia, provincia FROM provincias;", reader =>
        {
            var list = new List<Provincia>();
            while (reader.Read())
            {
                list.Add(new Provincia { IdProvincia = (int)reader["id_provincia"], Nombre = (string)reader["provincia"] });
            }
            return list;
        });

        public List<Partido> GetPartidosByProvinciaId(int provinciaId) => ExecuteReader("SELECT id_partido, partido, id_provincia FROM partidos WHERE id_provincia = @id_provincia;", reader =>
        {
            var list = new List<Partido>();
            while (reader.Read())
            {
                list.Add(new Partido { IdPartido = (int)reader["id_partido"], Nombre = (string)reader["partido"], IdProvincia = (int)reader["id_provincia"] });
            }
            return list;
        }, p => p.AddWithValue("@id_provincia", provinciaId));

        public List<Localidad> GetLocalidadesByPartidoId(int partidoId) => ExecuteReader("SELECT id_localidad, localidad, id_partido FROM localidades WHERE id_partido = @id_partido;", reader =>
        {
            var list = new List<Localidad>();
            while (reader.Read())
            {
                list.Add(new Localidad { IdLocalidad = (int)reader["id_localidad"], Nombre = (string)reader["localidad"], IdPartido = (int)reader["id_partido"] });
            }
            return list;
        }, p => p.AddWithValue("@id_partido", partidoId));

        public TipoDoc? GetTipoDocByNombre(string nombre) => ExecuteReader("SELECT id_tipo_doc, tipo_doc FROM tipo_doc WHERE tipo_doc = @nombre;", reader =>
        {
            if (!reader.Read()) return null;
            return new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = (string)reader["tipo_doc"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Localidad? GetLocalidadByNombre(string nombre) => ExecuteReader("SELECT id_localidad, localidad, id_partido FROM localidades WHERE localidad = @nombre;", reader =>
        {
            if (!reader.Read()) return null;
            return new Localidad { IdLocalidad = (int)reader["id_localidad"], Nombre = (string)reader["localidad"], IdPartido = (int)reader["id_partido"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Genero? GetGeneroByNombre(string nombre) => ExecuteReader("SELECT id_genero, genero FROM generos WHERE genero = @nombre;", reader =>
        {
            if (!reader.Read()) return null;
            return new Genero { IdGenero = (int)reader["id_genero"], Nombre = (string)reader["genero"] };
        }, p => p.AddWithValue("@nombre", nombre));

        public Rol? GetRolByNombre(string nombre) => ExecuteReader("SELECT id_rol, rol FROM roles WHERE rol = @nombre;", reader =>
        {
            if (!reader.Read()) return null;
            return new Rol { IdRol = (int)reader["id_rol"], Nombre = (string)reader["rol"] };
        }, p => p.AddWithValue("@nombre", nombre));
    }
}
