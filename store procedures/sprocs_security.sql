-- GetPoliticaSeguridad
CREATE PROCEDURE sp_get_politica_seguridad
AS
BEGIN
    SELECT TOP 1 * FROM politicas_seguridad;
END
GO

-- GetPreguntasSeguridad
CREATE PROCEDURE sp_get_preguntas_seguridad
AS
BEGIN
    SELECT id_pregunta, pregunta FROM preguntas_seguridad;
END
GO

-- GetPreguntasSeguridadByIds
CREATE PROCEDURE sp_get_preguntas_seguridad_by_ids
    @ids NVARCHAR(MAX)
AS
BEGIN
    SELECT id_pregunta, pregunta
    FROM preguntas_seguridad
    WHERE id_pregunta IN (SELECT value FROM STRING_SPLIT(@ids, ','));
END
GO

-- GetRespuestasSeguridadByUsuarioId
CREATE PROCEDURE sp_get_respuestas_seguridad_by_usuario_id
    @id_usuario INT
AS
BEGIN
    SELECT id_usuario, id_pregunta, respuesta
    FROM respuestas_seguridad
    WHERE id_usuario = @id_usuario;
END
GO

-- DeleteRespuestasSeguridadByUsuarioId
CREATE PROCEDURE sp_delete_respuestas_seguridad_by_usuario_id
    @id_usuario INT
AS
BEGIN
    DELETE FROM respuestas_seguridad
    WHERE id_usuario = @id_usuario;
END
GO
