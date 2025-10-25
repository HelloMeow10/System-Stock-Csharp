-- GetAllTiposDoc
CREATE PROCEDURE sp_get_all_tipos_doc
AS
BEGIN
    SELECT id_tipo_doc, tipo_doc FROM tipo_doc;
END
GO

-- GetAllGeneros
CREATE PROCEDURE sp_get_all_generos
AS
BEGIN
    SELECT id_genero, genero FROM generos;
END
GO

-- GetAllRoles
CREATE PROCEDURE sp_get_all_roles
AS
BEGIN
    SELECT id_rol, rol FROM roles;
END
GO

-- GetAllProvincias
CREATE PROCEDURE sp_get_all_provincias
AS
BEGIN
    SELECT id_provincia, provincia FROM provincias;
END
GO

-- GetPartidosByProvinciaId
CREATE PROCEDURE sp_get_partidos_by_provincia_id
    @id_provincia INT
AS
BEGIN
    SELECT id_partido, partido, id_provincia
    FROM partidos
    WHERE id_provincia = @id_provincia;
END
GO

-- GetLocalidadesByPartidoId
CREATE PROCEDURE sp_get_localidades_by_partido_id
    @id_partido INT
AS
BEGIN
    SELECT id_localidad, localidad, id_partido
    FROM localidades
    WHERE id_partido = @id_partido;
END
GO

-- GetTipoDocByNombre
CREATE PROCEDURE sp_get_tipo_doc_by_nombre
    @nombre VARCHAR(255)
AS
BEGIN
    SELECT id_tipo_doc, tipo_doc
    FROM tipo_doc
    WHERE tipo_doc = @nombre;
END
GO

-- GetLocalidadByNombre
CREATE PROCEDURE sp_get_localidad_by_nombre
    @nombre VARCHAR(255)
AS
BEGIN
    SELECT id_localidad, localidad, id_partido
    FROM localidades
    WHERE localidad = @nombre;
END
GO

-- GetGeneroByNombre
CREATE PROCEDURE sp_get_genero_by_nombre
    @nombre VARCHAR(255)
AS
BEGIN
    SELECT id_genero, genero
    FROM generos
    WHERE genero = @nombre;
END
GO

-- GetRolByNombre
CREATE PROCEDURE sp_get_rol_by_nombre
    @nombre VARCHAR(255)
AS
BEGIN
    SELECT id_rol, rol
    FROM roles
    WHERE rol = @nombre;
END
GO
