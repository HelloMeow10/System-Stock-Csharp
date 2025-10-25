-- GetPersonaById
CREATE PROCEDURE sp_get_persona_by_id
    @id INT
AS
BEGIN
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
    WHERE p.id_persona = @id;
END
GO

-- GetAllPersonas
CREATE PROCEDURE sp_get_all_personas
AS
BEGIN
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
        generos g ON p.id_genero = g.id_genero;
END
GO
