using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlPersonaRepository : IPersonaRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlPersonaRepository> _logger;

        public SqlPersonaRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlPersonaRepository> logger)
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

        private void ExecuteNonQuery(string sql, Action<SqlParameterCollection> addParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (var connection = (SqlConnection)_connectionFactory.CreateConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = commandType;
                        addParameters(command.Parameters);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Ocurrió un error de SQL al ejecutar ExecuteNonQuery para el comando: {sql}", sql);
                throw new DataAccessLayerException($"Ocurrió un error de SQL al ejecutar {sql}", ex);
            }
        }

        public Persona? GetPersonaById(int id)
        {
            var sql = @"
                SELECT
                    p.id_persona, p.legajo, p.nombre, p.apellido, p.id_tipo_doc, p.num_doc, p.fecha_nacimiento, p.cuil, p.calle, p.altura, p.id_localidad, p.id_genero, p.correo, p.celular, p.fecha_ingreso,
                    td.tipo_doc AS TipoDocNombre,
                    l.localidad AS LocalidadNombre,
                    pa.id_partido AS IdPartido,
                    pa.partido AS PartidoNombre,
                    pr.id_provincia AS IdProvincia,
                    pr.provincia AS ProvinciaNombre,
                    g.genero AS GeneroNombre
                FROM
                    personas p
                LEFT JOIN
                    tipo_doc td ON p.id_tipo_doc = td.id_tipo_doc
                LEFT JOIN
                    localidades l ON p.id_localidad = l.id_localidad
                LEFT JOIN
                    partidos pa ON l.id_partido = pa.id_partido
                LEFT JOIN
                    provincias pr ON pa.id_provincia = pr.id_provincia
                LEFT JOIN
                    generos g ON p.id_genero = g.id_genero
                WHERE p.id_persona = @id";

            return ExecuteReader(sql, reader =>
            {
                if (!reader.Read()) return null;

                var persona = new Persona(
                    (int)reader["legajo"],
                    reader["nombre"].ToString()!,
                    reader["apellido"].ToString()!,
                    (int)reader["id_tipo_doc"],
                    reader["num_doc"].ToString()!,
                    reader["fecha_nacimiento"] as DateTime?,
                    reader["cuil"] as string,
                    reader["calle"] as string,
                    reader["altura"] as string,
                    (int)reader["id_localidad"],
                    (int)reader["id_genero"],
                    reader["correo"] as string,
                    reader["celular"] as string,
                    (DateTime)reader["fecha_ingreso"]
                )
                {
                    IdPersona = (int)reader["id_persona"],
                    TipoDoc = new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = reader["TipoDocNombre"] as string ?? string.Empty },
                    Genero = new Genero { IdGenero = (int)reader["id_genero"], Nombre = reader["GeneroNombre"] as string ?? string.Empty }
                };

                var localidad = new Localidad
                {
                    IdLocalidad = (int)reader["id_localidad"],
                    Nombre = reader["LocalidadNombre"] as string ?? string.Empty
                };

                if (reader["IdPartido"] != DBNull.Value)
                {
                    var idPartido = (int)reader["IdPartido"];
                    var partido = new Partido
                    {
                        IdPartido = idPartido,
                        Nombre = reader["PartidoNombre"] as string ?? string.Empty
                    };

                    if (reader["IdProvincia"] != DBNull.Value)
                    {
                        var idProvincia = (int)reader["IdProvincia"];
                        partido.IdProvincia = idProvincia;
                        partido.Provincia = new Provincia
                        {
                            IdProvincia = idProvincia,
                            Nombre = reader["ProvinciaNombre"] as string ?? string.Empty
                        };
                    }

                    localidad.IdPartido = idPartido;
                    localidad.Partido = partido;
                }
                else
                {
                    localidad.IdPartido = 0;
                    localidad.Partido = null!;
                }

                persona.Localidad = localidad;
                return persona;
            }, p => p.AddWithValue("@id", id));
        }

        public List<Persona> GetAllPersonas()
        {
            var sql = @"
                SELECT
                    p.id_persona, p.legajo, p.nombre, p.apellido, p.id_tipo_doc, p.num_doc, p.fecha_nacimiento, p.cuil, p.calle, p.altura, p.id_localidad, p.id_genero, p.correo, p.celular, p.fecha_ingreso,
                    td.tipo_doc AS TipoDocNombre,
                    l.localidad AS LocalidadNombre,
                    pa.id_partido AS IdPartido,
                    pa.partido AS PartidoNombre,
                    pr.id_provincia AS IdProvincia,
                    pr.provincia AS ProvinciaNombre,
                    g.genero AS GeneroNombre
                FROM
                    personas p
                LEFT JOIN
                    tipo_doc td ON p.id_tipo_doc = td.id_tipo_doc
                LEFT JOIN
                    localidades l ON p.id_localidad = l.id_localidad
                LEFT JOIN
                    partidos pa ON l.id_partido = pa.id_partido
                LEFT JOIN
                    provincias pr ON pa.id_provincia = pr.id_provincia
                LEFT JOIN
                    generos g ON p.id_genero = g.id_genero";

            return ExecuteReader(sql, reader =>
            {
                var personas = new List<Persona>();
                while (reader.Read())
                {
                    var persona = new Persona(
                        (int)reader["legajo"],
                        reader["nombre"].ToString()!,
                        reader["apellido"].ToString()!,
                        (int)reader["id_tipo_doc"],
                        reader["num_doc"].ToString()!,
                        reader["fecha_nacimiento"] as DateTime?,
                        reader["cuil"] as string,
                        reader["calle"] as string,
                        reader["altura"] as string,
                        (int)reader["id_localidad"],
                        (int)reader["id_genero"],
                        reader["correo"] as string,
                        reader["celular"] as string,
                        (DateTime)reader["fecha_ingreso"]
                    )
                    {
                        IdPersona = (int)reader["id_persona"],
                        TipoDoc = new TipoDoc { IdTipoDoc = (int)reader["id_tipo_doc"], Nombre = reader["TipoDocNombre"] as string ?? string.Empty },
                        Genero = new Genero { IdGenero = (int)reader["id_genero"], Nombre = reader["GeneroNombre"] as string ?? string.Empty }
                    };

                    var localidad = new Localidad
                    {
                        IdLocalidad = (int)reader["id_localidad"],
                        Nombre = reader["LocalidadNombre"] as string ?? string.Empty
                    };

                    if (reader["IdPartido"] != DBNull.Value)
                    {
                        var idPartido = (int)reader["IdPartido"];
                        var partido = new Partido
                        {
                            IdPartido = idPartido,
                            Nombre = reader["PartidoNombre"] as string ?? string.Empty
                        };

                        if (reader["IdProvincia"] != DBNull.Value)
                        {
                            var idProvincia = (int)reader["IdProvincia"];
                            partido.IdProvincia = idProvincia;
                            partido.Provincia = new Provincia
                            {
                                IdProvincia = idProvincia,
                                Nombre = reader["ProvinciaNombre"] as string ?? string.Empty
                            };
                        }

                        localidad.IdPartido = idPartido;
                        localidad.Partido = partido;
                    }
                    else
                    {
                        localidad.IdPartido = 0;
                        localidad.Partido = null!;
                    }

                    persona.Localidad = localidad;
                    personas.Add(persona);
                }
                return personas;
            });
        }

        public void AddPersona(Persona persona)
        {
            _logger.LogInformation("Agregando persona a la base de datos: Legajo={Legajo}", persona.Legajo);
            ExecuteNonQuery("sp_insert_persona", p =>
            {
                p.AddWithValue("@legajo", persona.Legajo);
                p.AddWithValue("@nombre", persona.Nombre);
                p.AddWithValue("@apellido", persona.Apellido);
                p.AddWithValue("@id_tipo_doc", persona.IdTipoDoc);
                p.AddWithValue("@num_doc", persona.NumDoc);
                p.AddWithValue("@fecha_nacimiento", (object?)persona.FechaNacimiento ?? DBNull.Value);
                p.AddWithValue("@cuil", (object?)persona.Cuil ?? DBNull.Value);
                p.AddWithValue("@calle", (object?)persona.Calle ?? DBNull.Value);
                p.AddWithValue("@altura", (object?)persona.Altura ?? DBNull.Value);
                p.AddWithValue("@id_localidad", persona.IdLocalidad);
                p.AddWithValue("@id_genero", persona.IdGenero);
                p.AddWithValue("@correo", (object?)persona.Correo ?? DBNull.Value);
                p.AddWithValue("@celular", (object?)persona.Celular ?? DBNull.Value);
            });
            _logger.LogInformation("Persona con legajo {Legajo} agregada exitosamente.", persona.Legajo);
        }

        public void UpdatePersona(Persona persona) => ExecuteNonQuery("sp_update_persona", p =>
        {
            p.AddWithValue("@id_persona", persona.IdPersona);
            p.AddWithValue("@legajo", persona.Legajo);
            p.AddWithValue("@nombre", persona.Nombre);
            p.AddWithValue("@apellido", persona.Apellido);
            p.AddWithValue("@id_tipo_doc", persona.IdTipoDoc);
            p.AddWithValue("@num_doc", persona.NumDoc);
            p.AddWithValue("@fecha_nacimiento", (object?)persona.FechaNacimiento ?? DBNull.Value);
            p.AddWithValue("@cuil", (object?)persona.Cuil ?? DBNull.Value);
            p.AddWithValue("@calle", (object?)persona.Calle ?? DBNull.Value);
            p.AddWithValue("@altura", (object?)persona.Altura ?? DBNull.Value);
            p.AddWithValue("@id_localidad", persona.IdLocalidad);
            p.AddWithValue("@id_genero", persona.IdGenero);
            p.AddWithValue("@correo", (object?)persona.Correo ?? DBNull.Value);
            p.AddWithValue("@celular", (object?)persona.Celular ?? DBNull.Value);
            p.AddWithValue("@fecha_ingreso", (object?)persona.FechaIngreso ?? DBNull.Value);
        });

        public void DeletePersona(int personaId) => ExecuteNonQuery("sp_delete_persona", p =>
        {
            p.AddWithValue("@id_persona", personaId);
        });
    }
}
