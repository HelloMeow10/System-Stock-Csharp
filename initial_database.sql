-- Step 1: Terminate all connections to the login2 database
IF DB_ID('login2') IS NOT NULL
BEGIN
    -- Set database to single-user mode to terminate connections
    ALTER DATABASE login2 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    -- Drop the database
    DROP DATABASE login2;
END
GO

-- Step 2: Create the new login2 database
CREATE DATABASE login2;
GO

-- Step 3: Use the login2 database
USE login2;
GO

-- Step 4: Create Tables
-- 1. Provincias
CREATE TABLE provincias (
    id_provincia INT PRIMARY KEY IDENTITY(1,1),
    provincia VARCHAR(50) NOT NULL
);

-- 2. Partidos
CREATE TABLE partidos (
    id_partido INT PRIMARY KEY IDENTITY(1,1),
    partido VARCHAR(50) NOT NULL,
    id_provincia INT NOT NULL,
    FOREIGN KEY (id_provincia) REFERENCES provincias(id_provincia)
);

-- 3. Localidades
CREATE TABLE localidades (
    id_localidad INT PRIMARY KEY IDENTITY(1,1),
    localidad VARCHAR(50) NOT NULL,
    id_partido INT NOT NULL,
    FOREIGN KEY (id_partido) REFERENCES partidos(id_partido)
);

-- 4. Tipos de documento
CREATE TABLE tipo_doc (
    id_tipo_doc INT PRIMARY KEY IDENTITY(1,1),
    tipo_doc VARCHAR(30) NOT NULL
);

-- 5. Géneros
CREATE TABLE generos (
    id_genero INT PRIMARY KEY IDENTITY(1,1),
    genero VARCHAR(25) NOT NULL
);

-- 6. Personas
CREATE TABLE personas (
    id_persona INT PRIMARY KEY IDENTITY(1,1),
    legajo INT NOT NULL,
    nombre VARCHAR(30) NOT NULL,
    apellido VARCHAR(30) NOT NULL,
    id_tipo_doc INT NOT NULL,
    num_doc VARCHAR(20) NOT NULL,
    fecha_nacimiento DATE NULL,
    cuil VARCHAR(15),
    calle VARCHAR(50),
    altura VARCHAR(30),   
    id_localidad INT NOT NULL,
    id_genero INT NOT NULL,
    correo VARCHAR(100),
    celular VARCHAR(30),
    fecha_ingreso DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (id_tipo_doc) REFERENCES tipo_doc(id_tipo_doc),
    FOREIGN KEY (id_localidad) REFERENCES localidades(id_localidad),
    FOREIGN KEY (id_genero) REFERENCES generos(id_genero)
);

-- 7. Roles (was 8)
CREATE TABLE roles (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    rol VARCHAR(50) NOT NULL
);

-- 8. Políticas de Seguridad (was 9)
CREATE TABLE politicas_seguridad (
    id_politica INT PRIMARY KEY IDENTITY(1,1),
    min_caracteres INT,
    cant_preguntas INT,
    mayus_y_minus BIT,
    letras_y_numeros BIT,
    caracter_especial BIT,
    autenticacion_2fa BIT,
    no_repetir_anteriores BIT,
    sin_datos_personales BIT
);

-- Insert Default Security Policy
INSERT INTO politicas_seguridad (min_caracteres, cant_preguntas, mayus_y_minus, letras_y_numeros, caracter_especial, autenticacion_2fa, no_repetir_anteriores, sin_datos_personales)
VALUES (8, 3, 0, 0, 0, 0, 0, 0);
GO

-- 9. Usuarios (was 10)
CREATE TABLE usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    usuario VARCHAR(30) NOT NULL,
    contrasena_script VARBINARY(512) NOT NULL,
    id_persona INT NOT NULL,
    fecha_bloqueo DATETIME NOT NULL DEFAULT GETDATE(),
    nombre_usuario_bloqueo VARCHAR(30),
    fecha_ultimo_cambio DATETIME NOT NULL DEFAULT GETDATE(),
    id_rol INT NOT NULL,
    id_politica INT,
    CambioContrasenaObligatorio BIT NOT NULL DEFAULT 0,
    Codigo2FA VARCHAR(10),
    Codigo2FAExpiracion DATETIME,
    FechaExpiracion DATETIME NULL,
    FOREIGN KEY (id_persona) REFERENCES personas(id_persona),
    FOREIGN KEY (id_rol) REFERENCES roles(id_rol),
    FOREIGN KEY (id_politica) REFERENCES politicas_seguridad(id_politica)
);

-- 10. Permisos (was 11)
CREATE TABLE permisos (
    id_permiso INT PRIMARY KEY IDENTITY(1,1),
    permiso VARCHAR(50) NOT NULL,
    descripcion VARCHAR(200)
);

-- 11. Rol - Permiso (was 12)
CREATE TABLE rol_permiso (
    id_rol INT NOT NULL,
    id_permiso INT NOT NULL,
    PRIMARY KEY (id_rol, id_permiso),
    FOREIGN KEY (id_rol) REFERENCES roles(id_rol),
    FOREIGN KEY (id_permiso) REFERENCES permisos(id_permiso)
);

-- 12. Usuario - Permiso (was 13)
CREATE TABLE permisos_usuarios (
    Id INT NOT NULL,
    id_permiso INT NOT NULL,
    fecha_vencimiento DATE,
    PRIMARY KEY (Id, id_permiso),
    FOREIGN KEY (Id) REFERENCES usuarios(Id),
    FOREIGN KEY (id_permiso) REFERENCES permisos(id_permiso)
);

-- 13. Historial de Contraseñas (was 14)
CREATE TABLE historial_contrasena (
    id INT PRIMARY KEY IDENTITY(1,1),
    Id INT NOT NULL,
    fecha_cambio DATETIME NOT NULL DEFAULT GETDATE(),
    contrasena_script VARBINARY(512) NOT NULL,
    FOREIGN KEY (Id) REFERENCES usuarios(Id)
);

-- 14. Preguntas de Seguridad (was 15)
CREATE TABLE preguntas_seguridad (
    id_pregunta INT PRIMARY KEY IDENTITY(1,1),
    pregunta VARCHAR(255) NOT NULL
);

-- 15. Respuestas de Seguridad (was 16)
CREATE TABLE respuestas_seguridad (
    Id INT NOT NULL,
    id_pregunta INT NOT NULL,
    respuesta VARCHAR(60) NOT NULL,
    PRIMARY KEY (Id, id_pregunta),
    FOREIGN KEY (Id) REFERENCES usuarios(Id),
    FOREIGN KEY (id_pregunta) REFERENCES preguntas_seguridad(id_pregunta)
);

-- Step 5: Create Stored Procedures

-- 1. Provincias
DROP PROCEDURE IF EXISTS sp_insert_prov;
GO
CREATE PROCEDURE sp_insert_prov
    @provincia VARCHAR(50)
AS
BEGIN
    INSERT INTO provincias (provincia)
    VALUES (@provincia)
END
GO

DROP PROCEDURE IF EXISTS sp_update_prov;
GO
CREATE PROCEDURE sp_update_prov
    @id_provincia INT,
    @provincia VARCHAR(50)
AS
BEGIN
    UPDATE provincias
    SET provincia = @provincia
    WHERE id_provincia = @id_provincia
END
GO

-- 2. Partidos
DROP PROCEDURE IF EXISTS sp_insert_partido;
GO
CREATE PROCEDURE sp_insert_partido
    @partido VARCHAR(50),
    @id_provincia INT
AS
BEGIN
    INSERT INTO partidos (partido, id_provincia)
    VALUES (@partido, @id_provincia)
END
GO

DROP PROCEDURE IF EXISTS sp_update_partido;
GO
CREATE PROCEDURE sp_update_partido
    @id_partido INT,
    @partido VARCHAR(50),
    @id_provincia INT
AS
BEGIN
    UPDATE partidos
    SET partido = @partido,
        id_provincia = @id_provincia
    WHERE id_partido = @id_partido
END
GO

-- 3. Localidades
DROP PROCEDURE IF EXISTS sp_insert_localidad;
GO
CREATE PROCEDURE sp_insert_localidad
    @localidad VARCHAR(50),
    @id_partido INT
AS
BEGIN
    INSERT INTO localidades (localidad, id_partido)
    VALUES (@localidad, @id_partido)
END
GO

DROP PROCEDURE IF EXISTS sp_update_localidad;
GO
CREATE PROCEDURE sp_update_localidad
    @id_localidad INT,
    @localidad VARCHAR(50),
    @id_partido INT
AS
BEGIN
    UPDATE localidades
    SET localidad = @localidad,
        id_partido = @id_partido
    WHERE id_localidad = @id_localidad
END
GO

-- 4. Tipo_doc
DROP PROCEDURE IF EXISTS sp_insert_tipo_doc;
GO
CREATE PROCEDURE sp_insert_tipo_doc
    @tipo_doc VARCHAR(30)
AS
BEGIN
    INSERT INTO tipo_doc (tipo_doc)
    VALUES (@tipo_doc)
END
GO

DROP PROCEDURE IF EXISTS sp_update_tipo_doc;
GO
CREATE PROCEDURE sp_update_tipo_doc
    @id_tipo_doc INT,
    @tipo_doc VARCHAR(30)
AS
BEGIN
    UPDATE tipo_doc
    SET tipo_doc = @tipo_doc
    WHERE id_tipo_doc = @id_tipo_doc
END
GO

-- 5. Generos
DROP PROCEDURE IF EXISTS sp_insert_genero;
GO
CREATE PROCEDURE sp_insert_genero
    @genero VARCHAR(25)
AS
BEGIN
    INSERT INTO generos (genero)
    VALUES (@genero)
END
GO

DROP PROCEDURE IF EXISTS sp_update_genero;
GO
CREATE PROCEDURE sp_update_genero
    @id_genero INT,
    @genero VARCHAR(25)
AS
BEGIN
    UPDATE generos
    SET genero = @genero
    WHERE id_genero = @id_genero
END
GO

-- 6. Personas
DROP PROCEDURE IF EXISTS sp_insert_persona;
GO
CREATE PROCEDURE sp_insert_persona
    @legajo INT,
    @nombre VARCHAR(30),
    @apellido VARCHAR(30),
    @id_tipo_doc INT,
    @num_doc VARCHAR(20),
    @fecha_nacimiento DATE,
    @cuil VARCHAR(15),
    @calle VARCHAR(50),
    @altura VARCHAR(30),
    @id_localidad INT,
    @id_genero INT,
    @correo VARCHAR(100),
    @celular VARCHAR(30)
AS
BEGIN
    INSERT INTO personas (
        legajo, nombre, apellido, id_tipo_doc, num_doc, fecha_nacimiento,
        cuil, calle, altura, id_localidad, id_genero, correo, celular
    )
    VALUES (
        @legajo, @nombre, @apellido, @id_tipo_doc, @num_doc, @fecha_nacimiento,
        @cuil, @calle, @altura, @id_localidad, @id_genero, @correo, @celular
    )
END
GO

DROP PROCEDURE IF EXISTS sp_update_persona;
GO
CREATE PROCEDURE sp_update_persona
    @id_persona INT,
    @legajo INT,
    @nombre VARCHAR(30),
    @apellido VARCHAR(30),
    @id_tipo_doc INT,
    @num_doc VARCHAR(20),
    @fecha_nacimiento DATE,
    @cuil VARCHAR(15),
    @calle VARCHAR(50),
    @altura VARCHAR(30),
    @id_localidad INT,
    @id_genero INT,
    @correo VARCHAR(100),
    @celular VARCHAR(30),
    @fecha_ingreso DATETIME
AS
BEGIN
    UPDATE personas
    SET legajo = @legajo,
        nombre = @nombre,
        apellido = @apellido,
        id_tipo_doc = @id_tipo_doc,
        num_doc = @num_doc,
        fecha_nacimiento = @fecha_nacimiento,
        cuil = @cuil,
        calle = @calle,
        altura = @altura,
        id_localidad = @id_localidad,
        id_genero = @id_genero,
        correo = @correo,
        celular = @celular,
        fecha_ingreso = @fecha_ingreso
    WHERE id_persona = @id_persona
END
GO

-- 7. Roles (was 8)
DROP PROCEDURE IF EXISTS sp_insert_rol;
GO
CREATE PROCEDURE sp_insert_rol
    @rol VARCHAR(50)
AS
BEGIN
    INSERT INTO roles (rol)
    VALUES (@rol)
END
GO

DROP PROCEDURE IF EXISTS sp_update_rol;
GO
CREATE PROCEDURE sp_update_rol
    @id_rol INT,
    @rol VARCHAR(50)
AS
BEGIN
    UPDATE roles
    SET rol = @rol
    WHERE id_rol = @id_rol
END
GO

-- 8. Usuarios (was 9)
DROP PROCEDURE IF EXISTS sp_insert_usuario;
GO
CREATE PROCEDURE sp_insert_usuario
    @usuario VARCHAR(30),
    @contrasena_script VARBINARY(512),
    @id_persona INT,
    @fecha_bloqueo DATETIME,
    @nombre_usuario_bloqueo VARCHAR(30),
    @fecha_ultimo_cambio DATETIME,
    @id_rol INT,
    @CambioContrasenaObligatorio BIT = 0,
    @Codigo2FA VARCHAR(10) = NULL,
    @Codigo2FAExpiracion DATETIME = NULL,
    @FechaExpiracion DATETIME = NULL
AS
BEGIN
    INSERT INTO usuarios (
        usuario, contrasena_script, id_persona, fecha_bloqueo,
        nombre_usuario_bloqueo, fecha_ultimo_cambio, id_rol, CambioContrasenaObligatorio,
        Codigo2FA, Codigo2FAExpiracion, FechaExpiracion
    )
    VALUES (
        @usuario, @contrasena_script, @id_persona, @fecha_bloqueo,
        @nombre_usuario_bloqueo, @fecha_ultimo_cambio, @id_rol, @CambioContrasenaObligatorio,
        @Codigo2FA, @Codigo2FAExpiracion, @FechaExpiracion
    )
END
GO

DROP PROCEDURE IF EXISTS sp_actualizar_usuario;
GO
CREATE PROCEDURE sp_actualizar_usuario
    @Id INT,
    @usuario VARCHAR(30),
    @contrasena_script VARBINARY(512),
    @id_persona INT,
    @fecha_bloqueo DATETIME,
    @nombre_usuario_bloqueo VARCHAR(30),
    @fecha_ultimo_cambio DATETIME,
    @id_rol INT,
    @CambioContrasenaObligatorio BIT,
    @Codigo2FA VARCHAR(10) = NULL,
    @Codigo2FAExpiracion DATETIME = NULL,
    @FechaExpiracion DATETIME = NULL
AS
BEGIN
    UPDATE usuarios
    SET usuario = @usuario,
        contrasena_script = @contrasena_script,
        id_persona = @id_persona,
        fecha_bloqueo = @fecha_bloqueo,
        nombre_usuario_bloqueo = @nombre_usuario_bloqueo,
        fecha_ultimo_cambio = @fecha_ultimo_cambio,
        id_rol = @id_rol,
        CambioContrasenaObligatorio = @CambioContrasenaObligatorio,
        Codigo2FA = @Codigo2FA,
        Codigo2FAExpiracion = @Codigo2FAExpiracion,
        FechaExpiracion = @FechaExpiracion
    WHERE Id = @Id
END
GO

DROP PROCEDURE IF EXISTS sp_get_usuario_by_nombre;
GO
CREATE PROCEDURE sp_get_usuario_by_nombre
    @usuario_nombre VARCHAR(30)
AS
BEGIN
    SELECT
        u.Id,
        u.usuario,
        u.contrasena_script,
        u.id_persona,
        u.fecha_bloqueo,
        u.nombre_usuario_bloqueo,
        u.fecha_ultimo_cambio,
        u.id_rol,
        u.id_politica,
        u.CambioContrasenaObligatorio,
        u.Codigo2FA,
        u.Codigo2FAExpiracion,
        u.FechaExpiracion,
        r.id_rol AS rol_id_rol,
        r.rol
    FROM usuarios u
    INNER JOIN roles r ON u.id_rol = r.id_rol
    WHERE u.usuario = @usuario_nombre;
END
GO

DROP PROCEDURE IF EXISTS sp_get_all_users;
GO
CREATE PROCEDURE sp_get_all_users
AS
BEGIN
    SELECT
        u.Id,
        u.usuario,
        u.contrasena_script,
        u.id_persona,
        u.fecha_bloqueo,
        u.nombre_usuario_bloqueo,
        u.fecha_ultimo_cambio,
        u.id_rol,
        u.id_politica,
        u.CambioContrasenaObligatorio,
        u.Codigo2FA,
        u.Codigo2FAExpiracion,
        u.FechaExpiracion,
        r.id_rol AS rol_id_rol,
        r.rol
    FROM usuarios u
    INNER JOIN roles r ON u.id_rol = r.id_rol
END
GO

DROP PROCEDURE IF EXISTS sp_delete_usuario;
GO
CREATE PROCEDURE sp_delete_usuario
    @Id INT
AS
BEGIN
    BEGIN TRANSACTION;
    DELETE FROM permisos_usuarios WHERE Id = @Id;
    DELETE FROM historial_contrasena WHERE Id = @Id;
    DELETE FROM respuestas_seguridad WHERE Id = @Id;
    DELETE FROM usuarios WHERE Id = @Id;
    COMMIT TRANSACTION;
END
GO

-- 9. Permisos (was 10)
DROP PROCEDURE IF EXISTS sp_insert_permiso;
GO
CREATE PROCEDURE sp_insert_permiso
    @permiso VARCHAR(50),
    @descripcion VARCHAR(200)
AS
BEGIN
    INSERT INTO permisos (permiso, descripcion)
    VALUES (@permiso, @descripcion)
END
GO

DROP PROCEDURE IF EXISTS sp_update_permiso;
GO
CREATE PROCEDURE sp_update_permiso
    @id_permiso INT,
    @permiso VARCHAR(50),
    @descripcion VARCHAR(200)
AS
BEGIN
    UPDATE permisos
    SET permiso = @permiso,
        descripcion = @descripcion
    WHERE id_permiso = @id_permiso
END
GO

-- 10. Rol_permiso (was 11)
DROP PROCEDURE IF EXISTS sp_insert_rol_permiso;
GO
CREATE PROCEDURE sp_insert_rol_permiso
    @id_rol INT,
    @id_permiso INT
AS
BEGIN
    INSERT INTO rol_permiso (id_rol, id_permiso)
    VALUES (@id_rol, @id_permiso)
END
GO

-- 11. Permisos_usuarios (was 12)
DROP PROCEDURE IF EXISTS sp_insertar_permiso;
GO
CREATE PROCEDURE sp_insertar_permiso
    @Id INT,
    @id_permiso INT,
    @fecha_vencimiento DATE
AS
BEGIN
    INSERT INTO permisos_usuarios (Id, id_permiso, fecha_vencimiento)
    VALUES (@Id, @id_permiso, @fecha_vencimiento)
END
GO

DROP PROCEDURE IF EXISTS sp_update_permiso_usuario;
GO
CREATE PROCEDURE sp_update_permiso_usuario
    @Id INT,
    @id_permiso INT,
    @fecha_vencimiento DATE
AS
BEGIN
    UPDATE permisos_usuarios
    SET fecha_vencimiento = @fecha_vencimiento
    WHERE Id = @Id AND id_permiso = @id_permiso
END
GO

-- 12. Historial_contrasena (was 13)
DROP PROCEDURE IF EXISTS sp_historial_contrasena;
GO
CREATE PROCEDURE sp_historial_contrasena
    @Id INT,
    @contrasena_script VARBINARY(512)
AS
BEGIN
    INSERT INTO historial_contrasena (Id, contrasena_script)
    VALUES (@Id, @contrasena_script)
END
GO

-- 13. Preguntas_seguridad (was 14)
DROP PROCEDURE IF EXISTS sp_insert_pregunta_seguridad;
GO
CREATE PROCEDURE sp_insert_pregunta_seguridad
    @pregunta VARCHAR(255)
AS
BEGIN
    INSERT INTO preguntas_seguridad (pregunta)
    VALUES (@pregunta)
END
GO

DROP PROCEDURE IF EXISTS sp_update_pregunta_seguridad;
GO
CREATE PROCEDURE sp_update_pregunta_seguridad
    @id_pregunta INT,
    @pregunta VARCHAR(255)
AS
BEGIN
    UPDATE preguntas_seguridad
    SET pregunta = @pregunta
    WHERE id_pregunta = @id_pregunta
END
GO

-- 14. Respuestas_seguridad (was 15)
DROP PROCEDURE IF EXISTS sp_insert_respuesta_seguridad;
GO
CREATE PROCEDURE sp_insert_respuesta_seguridad
    @Id INT,
    @id_pregunta INT,
    @respuesta VARCHAR(60)
AS
BEGIN
    INSERT INTO respuestas_seguridad (Id, id_pregunta, respuesta)
    VALUES (@Id, @id_pregunta, @respuesta)
END
GO

DROP PROCEDURE IF EXISTS sp_update_respuesta_seguridad;
GO
CREATE PROCEDURE sp_update_respuesta_seguridad
    @Id INT,
    @id_pregunta INT,
    @respuesta VARCHAR(60)
AS
BEGIN
    UPDATE respuestas_seguridad
    SET respuesta = @respuesta
    WHERE Id = @Id AND id_pregunta = @id_pregunta
END
GO

-- 15. Politicas_seguridad (was 16)
DROP PROCEDURE IF EXISTS sp_insert_politica_seguridad;
GO
CREATE PROCEDURE sp_insert_politica_seguridad
    @min_caracteres INT,
    @cant_preguntas INT,
    @mayus_y_minus BIT,
    @letras_y_numeros BIT,
    @caracter_especial BIT,
    @autenticacion_2fa BIT,
    @no_repetir_anteriores BIT,
    @sin_datos_personales BIT
AS
BEGIN
    INSERT INTO politicas_seguridad (
        min_caracteres, cant_preguntas, mayus_y_minus, letras_y_numeros,
        caracter_especial, autenticacion_2fa, no_repetir_anteriores, sin_datos_personales
    )
    VALUES (
        @min_caracteres, @cant_preguntas, @mayus_y_minus, @letras_y_numeros,
        @caracter_especial, @autenticacion_2fa, @no_repetir_anteriores, @sin_datos_personales
    )
END
GO

DROP PROCEDURE IF EXISTS sp_update_politica_seguridad;
GO
CREATE PROCEDURE sp_update_politica_seguridad
    @id_politica INT,
    @min_caracteres INT,
    @cant_preguntas INT,
    @mayus_y_minus BIT,
    @letras_y_numeros BIT,
    @caracter_especial BIT,
    @autenticacion_2fa BIT,
    @no_repetir_anteriores BIT,
    @sin_datos_personales BIT
AS
BEGIN
    UPDATE politicas_seguridad
    SET min_caracteres = @min_caracteres,
        cant_preguntas = @cant_preguntas,
        mayus_y_minus = @mayus_y_minus,
        letras_y_numeros = @letras_y_numeros,
        caracter_especial = @caracter_especial,
        autenticacion_2fa = @autenticacion_2fa,
        no_repetir_anteriores = @no_repetir_anteriores,
        sin_datos_personales = @sin_datos_personales
    WHERE id_politica = @id_politica
END
GO

-- Step 6: Populate Initial Data with Robust Handling

-- tipo_doc
EXEC sp_insert_tipo_doc @tipo_doc = 'DNI';
EXEC sp_insert_tipo_doc @tipo_doc = 'Pasaporte';
GO

-- géneros
EXEC sp_insert_genero @genero = 'Masculino';
EXEC sp_insert_genero @genero = 'Femenino';
GO

-- Provincias
EXEC sp_insert_prov @provincia = 'CABA';
EXEC sp_insert_prov @provincia = 'Buenos Aires';
EXEC sp_insert_prov @provincia = 'Catamarca';
EXEC sp_insert_prov @provincia = 'Chaco';
EXEC sp_insert_prov @provincia = 'Chubut';
EXEC sp_insert_prov @provincia = 'Córdoba';
EXEC sp_insert_prov @provincia = 'Corrientes';
EXEC sp_insert_prov @provincia = 'Entre Ríos';
EXEC sp_insert_prov @provincia = 'Formosa';
EXEC sp_insert_prov @provincia = 'Jujuy';
EXEC sp_insert_prov @provincia = 'La Pampa';
EXEC sp_insert_prov @provincia = 'La Rioja';
EXEC sp_insert_prov @provincia = 'Mendoza';
EXEC sp_insert_prov @provincia = 'Misiones';
EXEC sp_insert_prov @provincia = 'Neuquén';
EXEC sp_insert_prov @provincia = 'Río Negro';
EXEC sp_insert_prov @provincia = 'Salta';
EXEC sp_insert_prov @provincia = 'San Juan';
EXEC sp_insert_prov @provincia = 'San Luis';
EXEC sp_insert_prov @provincia = 'Santa Cruz';
EXEC sp_insert_prov @provincia = 'Santa Fe';
EXEC sp_insert_prov @provincia = 'Santiago del Estero';
EXEC sp_insert_prov @provincia = 'Tierra del Fuego';
EXEC sp_insert_prov @provincia = 'Tucumán';
GO

-- Partidos
DECLARE @id_provincia INT;
SELECT @id_provincia = id_provincia FROM provincias WHERE provincia = 'Buenos Aires';
IF @id_provincia IS NOT NULL
BEGIN
    EXEC sp_insert_partido @partido = 'Almirante Brown', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Avellaneda', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Berazategui', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Esteban Echeverría', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Ezeiza', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Florencio Varela', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Lanús', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Lomas de Zamora', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Presidente Perón', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Quilmes', @id_provincia = @id_provincia;
END
ELSE
BEGIN
    RAISERROR ('Provincia "Buenos Aires" not found.', 16, 1);
END
GO

-- Localidades
-- Lanús
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Lanús';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Lanús Este', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Lanús Oeste', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Villa Caraza', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Remedios de Escalada', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Lanús" not found.', 16, 1);
END
GO

-- Avellaneda
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Avellaneda';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Avellaneda', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Dock Sud', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Sarandí', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Villa Domínico', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Avellaneda" not found.', 16, 1);
END
GO

-- Quilmes
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Quilmes';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Quilmes', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Bernal', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Ezpeleta', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'San Francisco Solano', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Quilmes" not found.', 16, 1);
END
GO

-- Lomas de Zamora
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Lomas de Zamora';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Lomas de Zamora', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Banfield', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Temperley', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Villa Centenario', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Lomas de Zamora" not found.', 16, 1);
END
GO

-- Almirante Brown
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Almirante Brown';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Adrogué', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Burzaco', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Claypole', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Glew', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Almirante Brown" not found.', 16, 1);
END
GO

-- Esteban Echeverría
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Esteban Echeverría';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Monte Grande', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'El Jagüel', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Canning', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Esteban Echeverría" not found.', 16, 1);
END
GO

-- Ezeiza
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Ezeiza';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Ezeiza', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Tristán Suárez', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Carlos Spegazzini', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Ezeiza" not found.', 16, 1);
END
GO

-- Florencio Varela
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Florencio Varela';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Florencio Varela', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Bosques', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Ingeniero Allan', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Florencio Varela" not found.', 16, 1);
END
GO

-- Presidente Perón
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Presidente Perón';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Guernica', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Presidente Perón" not found.', 16, 1);
END
GO

-- Berazategui
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Berazategui';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Berazategui', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Hudson', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Ranelagh', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Berazategui" not found.', 16, 1);
END
GO

-- Step 7: Insert Admin User
-- Insert Rol "Administrador"
EXEC sp_insert_rol @rol = 'Administrador';
EXEC sp_insert_rol @rol = 'Usuario';
GO

-- Insert Preguntas de Seguridad
EXEC sp_insert_pregunta_seguridad @pregunta = '¿Cuál era el nombre de tu primera mascota?';
EXEC sp_insert_pregunta_seguridad @pregunta = '¿Cuál es el apellido de soltera de tu madre?';
EXEC sp_insert_pregunta_seguridad @pregunta = '¿Cómo se llamaba tu escuela primaria?';
EXEC sp_insert_pregunta_seguridad @pregunta = '¿En qué ciudad naciste?';
EXEC sp_insert_pregunta_seguridad @pregunta = '¿Cuál es tu libro favorito?';
GO

-- Insert Persona for Admin
DECLARE @id_tipo_doc INT, @id_localidad INT, @id_genero INT;
SELECT @id_tipo_doc = id_tipo_doc FROM tipo_doc WHERE tipo_doc = 'DNI';
SELECT @id_localidad = id_localidad FROM localidades WHERE localidad = 'Lanús Este';
SELECT @id_genero = id_genero FROM generos WHERE genero = 'Masculino';

IF @id_tipo_doc IS NOT NULL AND @id_localidad IS NOT NULL AND @id_genero IS NOT NULL
BEGIN
    EXEC sp_insert_persona 
        @legajo = 1,
        @nombre = 'Admin',
        @apellido = 'User',
        @id_tipo_doc = @id_tipo_doc,
        @num_doc = '12345678',
        @fecha_nacimiento = '2000-01-01',
        @cuil = '20123456781',
        @calle = 'Admin St',
        @altura = '123',
        @id_localidad = @id_localidad,
        @id_genero = @id_genero,
        @correo = 'admin@example.com',
        @celular = '1122334455';
END
ELSE
BEGIN
    RAISERROR ('Initial data for persona not found.', 16, 1);
END
GO

-- Insert Usuario Admin
DECLARE @id_persona INT, @id_rol INT, @fecha_bloqueo DATETIME, @fecha_ultimo_cambio DATETIME;
SELECT @id_persona = id_persona FROM personas WHERE nombre = 'Admin';
SELECT @id_rol = id_rol FROM roles WHERE rol = 'Administrador';
SET @fecha_bloqueo = CAST('9999-12-31' AS DATETIME);
SET @fecha_ultimo_cambio = GETDATE();

IF @id_persona IS NOT NULL AND @id_rol IS NOT NULL
BEGIN
    -- Contraseña "admin123" encriptada con SHA256 (concatenada con 'admin')
    DECLARE @password VARBINARY(512) = HASHBYTES('SHA2_256', 'admin123admin');
    EXEC sp_insert_usuario
        @usuario = 'admin',
        @contrasena_script = @password,
        @id_persona = @id_persona,
        @fecha_bloqueo = @fecha_bloqueo,
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = @fecha_ultimo_cambio,
        @id_rol = @id_rol,
        @CambioContrasenaObligatorio = 1;
END
ELSE
BEGIN
    RAISERROR ('Persona or Rol for admin not found.', 16, 1);
END
GO

-- Stored Procedure to get password history for a user
CREATE PROCEDURE sp_get_historial_contrasenas_by_usuario_id
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id, Id, fecha_cambio, contrasena_script
    FROM historial_contrasena
    WHERE Id = @Id;
END
GO

-- Stored Procedure to set the 2FA code and expiry for a user
CREATE PROCEDURE sp_set_2fa_code
    @username NVARCHAR(255),
    @code NVARCHAR(10),
    @expiry DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE usuarios
    SET Codigo2FA = @code, Codigo2FAExpiracion = @expiry
    WHERE usuario = @username;
END
GO

DROP PROCEDURE IF EXISTS sp_get_users;
GO
CREATE PROCEDURE sp_get_users
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @Username NVARCHAR(30) = NULL,
    @Email NVARCHAR(100) = NULL,
    @SortBy NVARCHAR(100) = 'id_usuario',
    @TotalRecords INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Base query
    DECLARE @Query NVARCHAR(MAX);
    DECLARE @CountQuery NVARCHAR(MAX);
    DECLARE @WhereClause NVARCHAR(MAX) = N'';

    -- Build WHERE clause
    IF @Username IS NOT NULL AND @Username <> ''
    BEGIN
        SET @WhereClause = @WhereClause + ' AND u.usuario LIKE ''%'' + @Username + ''%''';
    END

    IF @Email IS NOT NULL AND @Email <> ''
    BEGIN
        -- We need to join with Personas table to filter by email
        SET @WhereClause = @WhereClause + ' AND p.correo LIKE ''%'' + @Email + ''%''';
    END

    -- Remove leading ' AND '
    IF LEN(@WhereClause) > 0
    BEGIN
        SET @WhereClause = SUBSTRING(@WhereClause, 6, LEN(@WhereClause));
    END
    ELSE
    BEGIN
        SET @WhereClause = '1=1'; -- No specific filter
    END

    -- Build the main query for fetching data
    SET @Query = N'
        SELECT
            u.id_usuario,
            u.usuario,
            u.contrasena_script,
            u.id_persona,
            u.fecha_bloqueo,
            u.nombre_usuario_bloqueo,
            u.fecha_ultimo_cambio,
            u.id_rol,
            u.id_politica,
            u.CambioContrasenaObligatorio,
            u.Codigo2FA,
            u.Codigo2FAExpiracion,
            u.FechaExpiracion,
            r.id_rol AS rol_id_rol,
            r.rol
        FROM
            usuarios u
        INNER JOIN
            roles r ON u.id_rol = r.id_rol
        INNER JOIN
            personas p ON u.id_persona = p.id_persona
        WHERE ' + @WhereClause + N'
        ORDER BY ' + @SortBy + N'
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;';

    -- Build the count query
    SET @CountQuery = N'
        SELECT @TotalRecords = COUNT(*)
        FROM
            usuarios u
        INNER JOIN
            personas p ON u.id_persona = p.id_persona
        WHERE ' + @WhereClause;

    -- Execute count query
    EXEC sp_executesql @CountQuery,
        N'@Username NVARCHAR(30), @Email NVARCHAR(100), @TotalRecords INT OUTPUT',
        @Username, @Email, @TotalRecords OUTPUT;

    -- Execute main query
    EXEC sp_executesql @Query,
        N'@PageNumber INT, @PageSize INT, @Username NVARCHAR(30), @Email NVARCHAR(100)',
        @PageNumber, @PageSize, @Username, @Email;

END
GO

-- ============================================
-- STOCK MANAGEMENT SCHEMA
-- ============================================

-- ============================================
-- TABLAS BASE
-- ============================================

CREATE TABLE motivoScrap (
    id_motivoScrap INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL
);

CREATE TABLE FormaPago (
    id_formaPago INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL
);

CREATE TABLE estadoCompras (
    id_estadoCompras INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL
);

CREATE TABLE EstadoVentas (
    id_estadoVentas INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL
);

CREATE TABLE Marcas (
    id_marca INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE CategoriasProducto (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200)
);

-- ============================================
-- PRODUCTOS
-- ============================================
CREATE TABLE Productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(50) NOT NULL,
    codBarras VARCHAR(50),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    id_marca INT,
    precioCompra DECIMAL(18,2),
    precioVenta DECIMAL(18,2),
    estado VARCHAR(50),
    ubicacion VARCHAR(100),
    habilitado BIT,
    id_categoria INT,
    FOREIGN KEY (id_marca) REFERENCES Marcas(id_marca),
    FOREIGN KEY (id_categoria) REFERENCES CategoriasProducto(id_categoria)
);

-- ============================================
-- PROVEEDORES
-- ============================================
CREATE TABLE Proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(50),
    nombre VARCHAR(100),
    razonSocial VARCHAR(150),
    CUIT VARCHAR(20),
    TiempoEntrega INT,
    Descuento DECIMAL(5,2),
    id_formaPago INT,
    FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);

CREATE TABLE ProveedorTelefonos (
    id_telefonoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(50),
    email VARCHAR(100),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

CREATE TABLE ProveedorUbicacion (
    id_ubicacionProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    direccion VARCHAR(150),
    localidad VARCHAR(100),
    provincia VARCHAR(100),
    tipo VARCHAR(50), -- Sucursal / Depósito
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- Relación Productos - Proveedores
CREATE TABLE ProductoProveedor (
    id_productoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_proveedor INT,
    precioCompra DECIMAL(18,2),
    tiempoEntrega INT,
    descuento DECIMAL(5,2),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- ============================================
-- COMPRAS
-- ============================================
CREATE TABLE Compras (
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    tipoDocumento VARCHAR(50), -- presupuesto / ordenCompra / Factura / notaDébito / notaCrédito
    numeroDocumento VARCHAR(50),
    montoTotal DECIMAL(18,2),
    id_estadoCompras INT,
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_estadoCompras) REFERENCES estadoCompras(id_estadoCompras)
);

CREATE TABLE DetalleCompras (
    id_detalleCompra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    FOREIGN KEY (id_compra) REFERENCES Compras(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- ============================================
-- CLIENTES
-- ============================================
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(50),
    nombre VARCHAR(100),
    razonSocial VARCHAR(150),
    CUIT_DNI VARCHAR(20),
    formaPago VARCHAR(50),
    limiteCredito DECIMAL(18,2),
    descuento DECIMAL(5,2),
    estado VARCHAR(50)
);

CREATE TABLE ClienteContactos (
    id_telefonoCliente INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(50),
    email VARCHAR(100),
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

CREATE TABLE ClienteDirecciones (
    id_direccion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    direccion VARCHAR(150),
    localidad VARCHAR(100),
    provincia VARCHAR(100),
    tipo VARCHAR(50), -- Sucursal / Depósito
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- ============================================
-- VENTAS
-- ============================================
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    fecha DATE,
    tipoDocumento VARCHAR(50),
    numeroDocumento VARCHAR(50),
    montoTotal DECIMAL(18,2),
    id_estadoVentas INT,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    FOREIGN KEY (id_estadoVentas) REFERENCES EstadoVentas(id_estadoVentas)
);

CREATE TABLE DetalleVentas (
    id_detalleVentas INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- ============================================
-- STOCK Y MOVIMIENTOS
-- ============================================
CREATE TABLE MovimientosStock (
    id_movimientosStock INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT,
    tipoMovimiento VARCHAR(50),
    fecha DATE,
    Id INT,
    id_producto INT,
    FOREIGN KEY (Id) REFERENCES usuarios(Id),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

CREATE TABLE Stock (
    id_stock INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    Id INT,
    lote VARCHAR(50),
    stock INT,
    stockMinimo INT,
    stockIdeal INT,
    stockMaximo INT,
    tipoStock VARCHAR(50), -- Existencia o JIT
    puntoReposicion INT,
    fechaVencimiento DATE,
    estadoHabilitaciones VARCHAR(50),
    id_movimientosStock INT,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (Id) REFERENCES usuarios(Id),
    FOREIGN KEY (id_movimientosStock) REFERENCES MovimientosStock(id_movimientosStock)
);

-- ============================================
-- SCRAP / BAJAS DE STOCK
-- ============================================
CREATE TABLE Scrap (
    id_scrap INT PRIMARY KEY IDENTITY(1,1),
    Id INT,
    fecha DATE,
    id_motivoScrap INT,
    FOREIGN KEY (Id) REFERENCES usuarios(Id),
    FOREIGN KEY (id_motivoScrap) REFERENCES motivoScrap(id_motivoScrap)
);
GO