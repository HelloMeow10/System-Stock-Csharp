using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using UserManagementSystem.DataAccess.Exceptions;

namespace DataAccess.Repositories
{
    public class SqlSecurityRepository : ISecurityRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlSecurityRepository> _logger;

        public SqlSecurityRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlSecurityRepository> logger)
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
                _logger.LogError(ex, "Ocurrió un error de SQL al ejecutar ExecuteNonQuery para el comando: {sql}", sql);
                throw new DataAccessLayerException($"Ocurrió un error de SQL al ejecutar {sql}", ex);
            }
        }

        public async Task<PoliticaSeguridad?> GetPoliticaSeguridadAsync() => await ExecuteReaderAsync("SELECT TOP 1 * FROM politicas_seguridad;", async reader =>
        {
            if (!await reader.ReadAsync()) return null;
            return new PoliticaSeguridad(
                (int)reader["id_politica"],
                reader["mayus_y_minus"] as bool? ?? false,
                reader["letras_y_numeros"] as bool? ?? false,
                reader["caracter_especial"] as bool? ?? false,
                reader["autenticacion_2fa"] as bool? ?? false,
                reader["no_repetir_anteriores"] as bool? ?? false,
                reader["sin_datos_personales"] as bool? ?? false,
                reader["min_caracteres"] as int? ?? 8,
                reader["cant_preguntas"] as int? ?? 3
            );
        });

        public async Task UpdatePoliticaSeguridadAsync(PoliticaSeguridad politica) => await ExecuteNonQueryAsync("sp_update_politica_seguridad", p =>
        {
            p.AddWithValue("@id_politica", politica.IdPolitica);
            p.AddWithValue("@min_caracteres", (object)politica.MinCaracteres ?? DBNull.Value);
            p.AddWithValue("@cant_preguntas", (object)politica.CantPreguntas ?? DBNull.Value);
            p.AddWithValue("@mayus_y_minus", (object)politica.MayusYMinus ?? DBNull.Value);
            p.AddWithValue("@letras_y_numeros", (object)politica.LetrasYNumeros ?? DBNull.Value);
            p.AddWithValue("@caracter_especial", (object)politica.CaracterEspecial ?? DBNull.Value);
            p.AddWithValue("@autenticacion_2fa", (object)politica.Autenticacion2FA ?? DBNull.Value);
            p.AddWithValue("@no_repetir_anteriores", (object)politica.NoRepetirAnteriores ?? DBNull.Value);
            p.AddWithValue("@sin_datos_personales", (object)politica.SinDatosPersonales ?? DBNull.Value);
        });

        public List<PreguntaSeguridad> GetPreguntasSeguridad() => ExecuteReader("SELECT id_pregunta, pregunta FROM preguntas_seguridad;", reader =>
        {
            var list = new List<PreguntaSeguridad>();
            while (reader.Read())
            {
                list.Add(new PreguntaSeguridad { IdPregunta = (int)reader["id_pregunta"], Pregunta = (string)reader["pregunta"] });
            }
            return list;
        });

        public List<PreguntaSeguridad> GetPreguntasSeguridadByIds(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<PreguntaSeguridad>();
            }

            var parameterNames = new List<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                parameterNames.Add($"@id{i}");
            }

            var sql = $"SELECT id_pregunta, pregunta FROM preguntas_seguridad WHERE id_pregunta IN ({string.Join(",", parameterNames)})";

            Action<SqlParameterCollection> addParametersAction = p =>
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    p.AddWithValue(parameterNames[i], ids[i]);
                }
            };

            return ExecuteReader(sql, reader =>
            {
                var list = new List<PreguntaSeguridad>();
                while (reader.Read())
                {
                    list.Add(new PreguntaSeguridad { IdPregunta = (int)reader["id_pregunta"], Pregunta = (string)reader["pregunta"] });
                }
                return list;
            }, addParametersAction, CommandType.Text);
        }

        public List<RespuestaSeguridad>? GetRespuestasSeguridadByUsuarioId(int idUsuario) => ExecuteReader("SELECT id_usuario, id_pregunta, respuesta FROM respuestas_seguridad WHERE id_usuario = @id_usuario;", reader =>
        {
            var list = new List<RespuestaSeguridad>();
            while (reader.Read())
            {
                list.Add(new RespuestaSeguridad { IdUsuario = (int)reader["id_usuario"], IdPregunta = (int)reader["id_pregunta"], Respuesta = (string)reader["respuesta"] });
            }
            return list;
        }, p => p.AddWithValue("@id_usuario", idUsuario));

        public void AddRespuestaSeguridad(RespuestaSeguridad respuesta) => ExecuteNonQuery("sp_insert_respuesta_seguridad", p =>
        {
            p.AddWithValue("@id_usuario", respuesta.IdUsuario);
            p.AddWithValue("@id_pregunta", respuesta.IdPregunta);
            p.AddWithValue("@respuesta", respuesta.Respuesta);
        });

        public void DeleteRespuestasSeguridadByUsuarioId(int usuarioId) => ExecuteNonQuery(
            "DELETE FROM respuestas_seguridad WHERE id_usuario = @id_usuario",
            p => p.AddWithValue("@id_usuario", usuarioId),
            CommandType.Text
        );
    }
}
