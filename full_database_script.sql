-- Eliminar item de presupuesto de compra
IF OBJECT_ID('sp_EliminarItemPresupuestoCompra', 'P') IS NOT NULL
    DROP PROCEDURE sp_EliminarItemPresupuestoCompra;
GO
CREATE PROCEDURE sp_EliminarItemPresupuestoCompra
    @id_detalle INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DetallePresupuestoCompra WHERE id_detalle = @id_detalle;
END
GO

-- Seed EstadoVentas básico para evitar FK inválida al crear ventas
IF NOT EXISTS (SELECT 1 FROM EstadoVentas)
BEGIN
    INSERT INTO EstadoVentas (facturada, entregada, cancelada) VALUES (0, 0, 0);
END
GO

-- Asegurar que exista id_estadoVentas = 1
IF NOT EXISTS (SELECT 1 FROM EstadoVentas WHERE id_estadoVentas = 1)
BEGIN
    SET IDENTITY_INSERT EstadoVentas ON;
    INSERT INTO EstadoVentas (id_estadoVentas, facturada, entregada, cancelada)
    VALUES (1, 0, 0, 0);
    SET IDENTITY_INSERT EstadoVentas OFF;
END
GO

-- Actualizar item de presupuesto de compra (cantidad / precio)
IF OBJECT_ID('sp_ActualizarItemPresupuestoCompra', 'P') IS NOT NULL
    DROP PROCEDURE sp_ActualizarItemPresupuestoCompra;
GO
CREATE PROCEDURE sp_ActualizarItemPresupuestoCompra
    @id_detalle INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DetallePresupuestoCompra
    SET cantidad = @cantidad,
        precioUnitario = @precioUnitario
    WHERE id_detalle = @id_detalle;
END
GO

-- Eliminar item de orden de compra
IF OBJECT_ID('sp_EliminarItemOrdenCompra', 'P') IS NOT NULL
    DROP PROCEDURE sp_EliminarItemOrdenCompra;
GO
CREATE PROCEDURE sp_EliminarItemOrdenCompra
    @id_detalle INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DetalleOrdenCompra WHERE id_detalle = @id_detalle;
END
GO

-- Actualizar item de orden de compra (cantidad / precio)
IF OBJECT_ID('sp_ActualizarItemOrdenCompra', 'P') IS NOT NULL
    DROP PROCEDURE sp_ActualizarItemOrdenCompra;
GO
CREATE PROCEDURE sp_ActualizarItemOrdenCompra
    @id_detalle INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DetalleOrdenCompra
    SET cantidad = @cantidad,
        precioUnitario = @precioUnitario
    WHERE id_detalle = @id_detalle;
END
GO
USE master;
GO

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

-- 5. GÃ©neros
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

-- 8. PolÃ­ticas de Seguridad (was 9)
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
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
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
    id_usuario INT NOT NULL,
    id_permiso INT NOT NULL,
    fecha_vencimiento DATE,
    PRIMARY KEY (id_usuario, id_permiso),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_permiso) REFERENCES permisos(id_permiso)
);

-- 13. Historial de ContraseÃ±as (was 14)
CREATE TABLE historial_contrasena (
    id INT PRIMARY KEY IDENTITY(1,1),
    id_usuario INT NOT NULL,
    fecha_cambio DATETIME NOT NULL DEFAULT GETDATE(),
    contrasena_script VARBINARY(512) NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

-- 14. Preguntas de Seguridad (was 15)
CREATE TABLE preguntas_seguridad (
    id_pregunta INT PRIMARY KEY IDENTITY(1,1),
    pregunta VARCHAR(255) NOT NULL
);

-- 15. Respuestas de Seguridad (was 16)
CREATE TABLE respuestas_seguridad (
    id_usuario INT NOT NULL,
    id_pregunta INT NOT NULL,
    respuesta VARCHAR(60) NOT NULL,
    PRIMARY KEY (id_usuario, id_pregunta),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
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
    @id_usuario INT,
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
    WHERE id_usuario = @id_usuario
END
GO

DROP PROCEDURE IF EXISTS sp_get_usuario_by_nombre;
GO
CREATE PROCEDURE sp_get_usuario_by_nombre
    @usuario_nombre VARCHAR(30)
AS
BEGIN
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
    FROM usuarios u
    INNER JOIN roles r ON u.id_rol = r.id_rol
    WHERE u.usuario = @usuario_nombre;
END
GO

DROP PROCEDURE IF EXISTS sp_get_usuario_by_id;
GO
CREATE PROCEDURE sp_get_usuario_by_id
    @id_usuario INT
AS
BEGIN
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
    FROM usuarios u
    INNER JOIN roles r ON u.id_rol = r.id_rol
    WHERE u.id_usuario = @id_usuario;
END
GO

DROP PROCEDURE IF EXISTS sp_get_all_users;
GO
CREATE PROCEDURE sp_get_all_users
AS
BEGIN
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
    FROM usuarios u
    INNER JOIN roles r ON u.id_rol = r.id_rol
END
GO

DROP PROCEDURE IF EXISTS sp_delete_usuario;
GO
CREATE PROCEDURE sp_delete_usuario
    @id_usuario INT
AS
BEGIN
    BEGIN TRANSACTION;
    DELETE FROM permisos_usuarios WHERE id_usuario = @id_usuario;
    DELETE FROM historial_contrasena WHERE id_usuario = @id_usuario;
    DELETE FROM respuestas_seguridad WHERE id_usuario = @id_usuario;
    DELETE FROM usuarios WHERE id_usuario = @id_usuario;
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
    @id_usuario INT,
    @id_permiso INT,
    @fecha_vencimiento DATE
AS
BEGIN
    INSERT INTO permisos_usuarios (id_usuario, id_permiso, fecha_vencimiento)
    VALUES (@id_usuario, @id_permiso, @fecha_vencimiento)
END
GO

DROP PROCEDURE IF EXISTS sp_update_permiso_usuario;
GO
CREATE PROCEDURE sp_update_permiso_usuario
    @id_usuario INT,
    @id_permiso INT,
    @fecha_vencimiento DATE
AS
BEGIN
    UPDATE permisos_usuarios
    SET fecha_vencimiento = @fecha_vencimiento
    WHERE id_usuario = @id_usuario AND id_permiso = @id_permiso
END
GO

-- 12. Historial_contrasena (was 13)
DROP PROCEDURE IF EXISTS sp_historial_contrasena;
GO
CREATE PROCEDURE sp_historial_contrasena
    @id_usuario INT,
    @contrasena_script VARBINARY(512)
AS
BEGIN
    INSERT INTO historial_contrasena (id_usuario, contrasena_script)
    VALUES (@id_usuario, @contrasena_script)
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
    SELECT SCOPE_IDENTITY() AS id_pregunta;
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

DROP PROCEDURE IF EXISTS sp_delete_pregunta_seguridad;
GO
CREATE PROCEDURE sp_delete_pregunta_seguridad
    @id_pregunta INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM respuestas_seguridad WHERE id_pregunta = @id_pregunta;
    DELETE FROM preguntas_seguridad WHERE id_pregunta = @id_pregunta;
END
GO

-- 14. Respuestas_seguridad (was 15)
DROP PROCEDURE IF EXISTS sp_insert_respuesta_seguridad;
GO
CREATE PROCEDURE sp_insert_respuesta_seguridad
    @id_usuario INT,
    @id_pregunta INT,
    @respuesta VARCHAR(60)
AS
BEGIN
    INSERT INTO respuestas_seguridad (id_usuario, id_pregunta, respuesta)
    VALUES (@id_usuario, @id_pregunta, @respuesta)
END
GO

DROP PROCEDURE IF EXISTS sp_update_respuesta_seguridad;
GO
CREATE PROCEDURE sp_update_respuesta_seguridad
    @id_usuario INT,
    @id_pregunta INT,
    @respuesta VARCHAR(60)
AS
BEGIN
    UPDATE respuestas_seguridad
    SET respuesta = @respuesta
    WHERE id_usuario = @id_usuario AND id_pregunta = @id_pregunta
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

-- gÃ©neros
EXEC sp_insert_genero @genero = 'Masculino';
EXEC sp_insert_genero @genero = 'Femenino';
GO

-- Provincias
EXEC sp_insert_prov @provincia = 'CABA';
EXEC sp_insert_prov @provincia = 'Buenos Aires';
EXEC sp_insert_prov @provincia = 'Catamarca';
EXEC sp_insert_prov @provincia = 'Chaco';
EXEC sp_insert_prov @provincia = 'Chubut';
EXEC sp_insert_prov @provincia = 'CÃ³rdoba';
EXEC sp_insert_prov @provincia = 'Corrientes';
EXEC sp_insert_prov @provincia = 'Entre RÃ­os';
EXEC sp_insert_prov @provincia = 'Formosa';
EXEC sp_insert_prov @provincia = 'Jujuy';
EXEC sp_insert_prov @provincia = 'La Pampa';
EXEC sp_insert_prov @provincia = 'La Rioja';
EXEC sp_insert_prov @provincia = 'Mendoza';
EXEC sp_insert_prov @provincia = 'Misiones';
EXEC sp_insert_prov @provincia = 'NeuquÃ©n';
EXEC sp_insert_prov @provincia = 'RÃ­o Negro';
EXEC sp_insert_prov @provincia = 'Salta';
EXEC sp_insert_prov @provincia = 'San Juan';
EXEC sp_insert_prov @provincia = 'San Luis';
EXEC sp_insert_prov @provincia = 'Santa Cruz';
EXEC sp_insert_prov @provincia = 'Santa Fe';
EXEC sp_insert_prov @provincia = 'Santiago del Estero';
EXEC sp_insert_prov @provincia = 'Tierra del Fuego';
EXEC sp_insert_prov @provincia = 'TucumÃ¡n';
GO

-- Partidos
DECLARE @id_provincia INT;
SELECT @id_provincia = id_provincia FROM provincias WHERE provincia = 'Buenos Aires';
IF @id_provincia IS NOT NULL
BEGIN
    EXEC sp_insert_partido @partido = 'Almirante Brown', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Avellaneda', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Berazategui', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Esteban EcheverrÃ­a', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Ezeiza', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Florencio Varela', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'LanÃºs', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Lomas de Zamora', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Presidente PerÃ³n', @id_provincia = @id_provincia;
    EXEC sp_insert_partido @partido = 'Quilmes', @id_provincia = @id_provincia;
END
ELSE
BEGIN
    RAISERROR ('Provincia "Buenos Aires" not found.', 16, 1);
END
GO

-- Localidades
-- LanÃºs
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'LanÃºs';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'LanÃºs Este', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'LanÃºs Oeste', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Villa Caraza', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Remedios de Escalada', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "LanÃºs" not found.', 16, 1);
END
GO

-- Avellaneda
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Avellaneda';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Avellaneda', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Dock Sud', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'SarandÃ­', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Villa DomÃ­nico', @id_partido = @id_partido;
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
    EXEC sp_insert_localidad @localidad = 'AdroguÃ©', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Burzaco', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Claypole', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Glew', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Almirante Brown" not found.', 16, 1);
END
GO

-- Esteban EcheverrÃ­a
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Esteban EcheverrÃ­a';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Monte Grande', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'El JagÃ¼el', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'Canning', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Esteban EcheverrÃ­a" not found.', 16, 1);
END
GO

-- Ezeiza
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Ezeiza';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Ezeiza', @id_partido = @id_partido;
    EXEC sp_insert_localidad @localidad = 'TristÃ¡n SuÃ¡rez', @id_partido = @id_partido;
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

-- Presidente PerÃ³n
DECLARE @id_partido INT;
SELECT @id_partido = id_partido FROM partidos WHERE partido = 'Presidente PerÃ³n';
IF @id_partido IS NOT NULL
BEGIN
    EXEC sp_insert_localidad @localidad = 'Guernica', @id_partido = @id_partido;
END
ELSE
BEGIN
    RAISERROR ('Partido "Presidente PerÃ³n" not found.', 16, 1);
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
EXEC sp_insert_pregunta_seguridad @pregunta = 'Â¿CuÃ¡l era el nombre de tu primera mascota?';
EXEC sp_insert_pregunta_seguridad @pregunta = 'Â¿CuÃ¡l es el apellido de soltera de tu madre?';
EXEC sp_insert_pregunta_seguridad @pregunta = 'Â¿CÃ³mo se llamaba tu escuela primaria?';
EXEC sp_insert_pregunta_seguridad @pregunta = 'Â¿En quÃ© ciudad naciste?';
EXEC sp_insert_pregunta_seguridad @pregunta = 'Â¿CuÃ¡l es tu libro favorito?';
GO

-- Insert Persona for Admin
DECLARE @id_tipo_doc INT, @id_localidad INT, @id_genero INT;
SELECT @id_tipo_doc = id_tipo_doc FROM tipo_doc WHERE tipo_doc = 'DNI';
SELECT @id_localidad = id_localidad FROM localidades WHERE localidad = 'LanÃºs Este';
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
SET @fecha_bloqueo = CAST('99991231' AS DATETIME);
SET @fecha_ultimo_cambio = GETDATE();

IF @id_persona IS NOT NULL AND @id_rol IS NOT NULL
BEGIN
    -- ContraseÃ±a "admin123" encriptada with Argon2id (salt=username)
    DECLARE @password VARBINARY(512) = 0xA6CD645C57030D00EB8F8CB4A2B21BBEDC54181871ACE4BB6E578D67337F4C05;
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
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id, id_usuario, fecha_cambio, contrasena_script
    FROM historial_contrasena
    WHERE id_usuario = @id_usuario;
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
    @RoleId INT = NULL,
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

    IF @RoleId IS NOT NULL
    BEGIN
        SET @WhereClause = @WhereClause + ' AND u.id_rol = @RoleId';
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
        N'@Username NVARCHAR(30), @Email NVARCHAR(100), @RoleId INT, @TotalRecords INT OUTPUT',
        @Username, @Email, @RoleId, @TotalRecords OUTPUT;

    -- Execute main query
    EXEC sp_executesql @Query,
        N'@PageNumber INT, @PageSize INT, @Username NVARCHAR(30), @Email NVARCHAR(100), @RoleId INT',
        @PageNumber, @PageSize, @Username, @Email, @RoleId;

END
GO

-- ============================================
-- ðŸ”¹ LIMPIEZA AUTOMÃTICA DE OBJETOS EXISTENTES
-- ============================================

/* Solo dropeo procedures listados (si existen) para evitar conflicto al recrearlos */
DROP PROCEDURE IF EXISTS sp_IngresoMercaderia;
DROP PROCEDURE IF EXISTS sp_ReporteMovimientosStock;
DROP PROCEDURE IF EXISTS sp_MoverStockAScrap;
DROP PROCEDURE IF EXISTS sp_ReporteScrap;
DROP PROCEDURE IF EXISTS sp_AgregarCliente;
DROP PROCEDURE IF EXISTS sp_ModificarCliente;
DROP PROCEDURE IF EXISTS sp_EliminarCliente;
DROP PROCEDURE IF EXISTS sp_ConsultarClientePorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarClientePorCUIT;
DROP PROCEDURE IF EXISTS sp_AgregarTelefonoCliente;
DROP PROCEDURE IF EXISTS sp_AgregarDireccionCliente;
DROP PROCEDURE IF EXISTS sp_RegistrarPresupuestoCompra;
DROP PROCEDURE IF EXISTS sp_RegistrarOrdenCompra;
DROP PROCEDURE IF EXISTS sp_ConsultarPedidos;
DROP PROCEDURE IF EXISTS sp_RegistrarRemito;
DROP PROCEDURE IF EXISTS sp_RegistrarFacturaCompra;
DROP PROCEDURE IF EXISTS sp_RegistrarNotaCredito;
DROP PROCEDURE IF EXISTS sp_RegistrarNotaDebito;
DROP PROCEDURE IF EXISTS sp_ReporteCompras;
DROP PROCEDURE IF EXISTS sp_ReporteDevolucionesProveedores;
DROP PROCEDURE IF EXISTS sp_AgregarProducto;
DROP PROCEDURE IF EXISTS sp_ModificarProducto;
DROP PROCEDURE IF EXISTS sp_EliminarProducto;
DROP PROCEDURE IF EXISTS sp_ObtenerProductos;
DROP PROCEDURE IF EXISTS sp_BuscarProducto;
DROP PROCEDURE IF EXISTS sp_ProductosHabilitados;
DROP PROCEDURE IF EXISTS sp_ProductosPorCategoria;
DROP PROCEDURE IF EXISTS sp_ProductosPorMarca;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorCodigo;
DROP PROCEDURE IF EXISTS sp_AgregarProveedor;
DROP PROCEDURE IF EXISTS sp_ModificarProveedor;
DROP PROCEDURE IF EXISTS sp_EliminarProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorCUIT;
DROP PROCEDURE IF EXISTS sp_AgregarTelefonoProveedor;
DROP PROCEDURE IF EXISTS sp_AgregarDireccionProveedor;
DROP PROCEDURE IF EXISTS sp_RelacionarProductoProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProductosPorProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorProducto;
DROP PROCEDURE IF EXISTS sp_AgregarCategoria;
DROP PROCEDURE IF EXISTS sp_ModificarCategoria;
DROP PROCEDURE IF EXISTS sp_EliminarCategoria;
DROP PROCEDURE IF EXISTS sp_AgregarMarca;
DROP PROCEDURE IF EXISTS sp_ModificarMarca;
DROP PROCEDURE IF EXISTS sp_EliminarMarca;
DROP PROCEDURE IF EXISTS sp_AgregarFormaPago;
DROP PROCEDURE IF EXISTS sp_ModificarFormaPago;
DROP PROCEDURE IF EXISTS sp_EliminarFormaPago;
DROP PROCEDURE IF EXISTS sp_ModificarEstadoCompra;
DROP PROCEDURE IF EXISTS sp_ModificarEstadoVenta;
DROP PROCEDURE IF EXISTS sp_AgregarPresupuestoVenta;
DROP PROCEDURE IF EXISTS sp_AgregarNotaPedido;
DROP PROCEDURE IF EXISTS sp_AgregarNotaCredito;
DROP PROCEDURE IF EXISTS sp_AgregarNotaDebito;
DROP PROCEDURE IF EXISTS sp_DevolverProductoStock;
DROP PROCEDURE IF EXISTS sp_ReporteVentas;
GO

-- ============================================
-- TABLAS BASE
-- ============================================

IF OBJECT_ID('dbo.MotivoScrap','U') IS NULL
BEGIN
CREATE TABLE MotivoScrap (
    id_motivoScrap INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(50),
    dano BIT DEFAULT 0,
    vencido BIT DEFAULT 0,
    obsoleto BIT DEFAULT 0,
    malaCalidad BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.FormaPago','U') IS NULL
BEGIN
CREATE TABLE FormaPago (
    id_formaPago INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL
);
END
GO

IF OBJECT_ID('dbo.EstadoCompras','U') IS NULL
BEGIN
CREATE TABLE EstadoCompras (
    id_estadoCompras INT IDENTITY(1,1) PRIMARY KEY,
    pendiente BIT DEFAULT 0,
    aprobada BIT DEFAULT 0,
    recibida BIT DEFAULT 0,
    cancelada BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.EstadoVentas','U') IS NULL
BEGIN
CREATE TABLE EstadoVentas (
    id_estadoVentas INT IDENTITY(1,1) PRIMARY KEY,
    facturada BIT DEFAULT 0,
    entregada BIT DEFAULT 0,  
    cancelada BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.MarcasProducto','U') IS NULL
BEGIN
CREATE TABLE MarcasProducto (
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(15) DEFAULT 'Habilitado',
    marca VARCHAR(45) NOT NULL
);
END
GO

IF OBJECT_ID('dbo.CategoriasProducto','U') IS NULL
BEGIN
CREATE TABLE CategoriasProducto (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(100) NOT NULL,
    descripcion VARCHAR(100),
    estado VARCHAR(15) DEFAULT 'Habilitado' 
);
END
GO

-- ============================================
-- PRODUCTOS
-- ============================================
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
CREATE TABLE Productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20) NOT NULL,
    codBarras VARCHAR(20),
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(200),
    id_marca INT,
    precioCompra DECIMAL(18,2),
    precioVenta DECIMAL(18,2),
    estado VARCHAR(25),
    ubicacion VARCHAR(100),
    habilitado BIT,
    id_categoria INT,
    unidadesAvisoVencimiento INT DEFAULT 0,
    unidadMedida VARCHAR(20) NULL,
    peso DECIMAL(18,2) NULL,
    volumen DECIMAL(18,2) NULL,
    puntoReposicion INT NULL,
    diasVencimiento INT NULL,
    loteObligatorio BIT NULL,
    controlVencimiento BIT NULL,
    CONSTRAINT FK_Productos_Marcas FOREIGN KEY (id_marca) REFERENCES MarcasProducto(id_marca),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (id_categoria) REFERENCES CategoriasProducto(id_categoria)
);
END
GO

-- ============================================
-- PROVEEDORES
-- ============================================
IF OBJECT_ID('dbo.Proveedores','U') IS NULL
BEGIN
CREATE TABLE Proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT VARCHAR(20),
    Email VARCHAR(100),
    Telefono VARCHAR(50),
    Direccion VARCHAR(150),
    Provincia VARCHAR(60),
    Ciudad VARCHAR(60),
    CondicionIVA VARCHAR(60),
    PlazoPagoDias INT,
    Observaciones VARCHAR(255),
    formaPago VARCHAR(100),
    TiempoEntrega INT,
    Descuento DECIMAL(5,2),
    id_formaPago INT,
    CONSTRAINT FK_Proveedores_FormaPago FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);
END
GO
DROP PROCEDURE IF EXISTS sp_GetAllProveedoresExtended;
DROP PROCEDURE IF EXISTS sp_GetProveedorByIdExtended;
DROP PROCEDURE IF EXISTS sp_SearchProveedoresByNombreExtended;
DROP PROCEDURE IF EXISTS sp_SearchProveedoresByCUITExtended;
DROP PROCEDURE IF EXISTS sp_AgregarProveedorExtended;
DROP PROCEDURE IF EXISTS sp_ModificarProveedorExtended;
DROP PROCEDURE IF EXISTS sp_EliminarProveedorExtended;
DROP PROCEDURE IF EXISTS sp_AgregarProveedorContacto;
DROP PROCEDURE IF EXISTS sp_GetProveedorContactos;
DROP PROCEDURE IF EXISTS sp_DeleteProveedorContactos;

-- Extended Supplier Procedures
GO
CREATE PROCEDURE sp_GetAllProveedoresExtended
AS
BEGIN
    SELECT id_proveedor, codigo, nombre, razonSocial, CUIT, Email, Telefono, Direccion, Provincia, Ciudad,
           CondicionIVA, PlazoPagoDias, Observaciones, TiempoEntrega, Descuento, id_formaPago
    FROM Proveedores;
END
GO

CREATE PROCEDURE sp_GetProveedorByIdExtended
    @id INT
AS
BEGIN
    SELECT id_proveedor, codigo, nombre, razonSocial, CUIT, Email, Telefono, Direccion, Provincia, Ciudad,
           CondicionIVA, PlazoPagoDias, Observaciones, TiempoEntrega, Descuento, id_formaPago
    FROM Proveedores
    WHERE id_proveedor = @id;
END
GO

CREATE PROCEDURE sp_SearchProveedoresByNombreExtended
    @nombre VARCHAR(50)
AS
BEGIN
    SELECT id_proveedor, codigo, nombre, razonSocial, CUIT, Email, Telefono, Direccion, Provincia, Ciudad,
           CondicionIVA, PlazoPagoDias, Observaciones, TiempoEntrega, Descuento, id_formaPago
    FROM Proveedores
    WHERE nombre LIKE '%' + @nombre + '%';
END
GO

CREATE PROCEDURE sp_SearchProveedoresByCUITExtended
    @cuit VARCHAR(20)
AS
BEGIN
    SELECT id_proveedor, codigo, nombre, razonSocial, CUIT, Email, Telefono, Direccion, Provincia, Ciudad,
           CondicionIVA, PlazoPagoDias, Observaciones, TiempoEntrega, Descuento, id_formaPago
    FROM Proveedores
    WHERE CUIT LIKE '%' + @cuit + '%';
END
GO

CREATE PROCEDURE sp_AgregarProveedorExtended
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @CUIT VARCHAR(20),
    @Email VARCHAR(100),
    @Telefono VARCHAR(50),
    @Direccion VARCHAR(150),
    @Provincia VARCHAR(60),
    @Ciudad VARCHAR(60),
    @CondicionIVA VARCHAR(60),
    @PlazoPagoDias INT,
    @Observaciones VARCHAR(255),
    @TiempoEntrega INT,
    @Descuento DECIMAL(5,2),
    @id_formaPago INT
AS
BEGIN
    INSERT INTO Proveedores(codigo, nombre, razonSocial, CUIT, Email, Telefono, Direccion, Provincia, Ciudad,
                            CondicionIVA, PlazoPagoDias, Observaciones, TiempoEntrega, Descuento, id_formaPago)
    VALUES(@codigo, @nombre, @razonSocial, @CUIT, @Email, @Telefono, @Direccion, @Provincia, @Ciudad,
           @CondicionIVA, @PlazoPagoDias, @Observaciones, @TiempoEntrega, @Descuento, @id_formaPago);
END
GO

CREATE PROCEDURE sp_ModificarProveedorExtended
    @id_proveedor INT,
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @CUIT VARCHAR(20),
    @Email VARCHAR(100),
    @Telefono VARCHAR(50),
    @Direccion VARCHAR(150),
    @Provincia VARCHAR(60),
    @Ciudad VARCHAR(60),
    @CondicionIVA VARCHAR(60),
    @PlazoPagoDias INT,
    @Observaciones VARCHAR(255),
    @TiempoEntrega INT,
    @Descuento DECIMAL(5,2),
    @id_formaPago INT
AS
BEGIN
    UPDATE Proveedores SET
        codigo = @codigo,
        nombre = @nombre,
        razonSocial = @razonSocial,
        CUIT = @CUIT,
        Email = @Email,
        Telefono = @Telefono,
        Direccion = @Direccion,
        Provincia = @Provincia,
        Ciudad = @Ciudad,
        CondicionIVA = @CondicionIVA,
        PlazoPagoDias = @PlazoPagoDias,
        Observaciones = @Observaciones,
        TiempoEntrega = @TiempoEntrega,
        Descuento = @Descuento,
        id_formaPago = @id_formaPago
    WHERE id_proveedor = @id_proveedor;
END
GO

CREATE PROCEDURE sp_EliminarProveedorExtended
    @id_proveedor INT
AS
BEGIN
    DELETE FROM Proveedores WHERE id_proveedor = @id_proveedor;
END
GO

CREATE PROCEDURE sp_GetProveedorContactos
    @codigo VARCHAR(50)
AS
BEGIN
    SELECT id_contactoProveedor, codigo, Nombre, Cargo, Email, Telefono
    FROM ProveedorContactos
    WHERE codigo = @codigo;
END
GO

CREATE PROCEDURE sp_DeleteProveedorContactos
    @codigo VARCHAR(50)
AS
BEGIN
    DELETE FROM ProveedorContactos WHERE codigo = @codigo;
END
GO

IF OBJECT_ID('dbo.ProveedorTelefonos','U') IS NULL
BEGIN
CREATE TABLE ProveedorTelefonos (
    id_telefonoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    contacto VARCHAR(50),
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(50),
    email VARCHAR(100),
    CONSTRAINT FK_ProveedorTelefonos_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.ProveedorUbicacion','U') IS NULL
BEGIN
CREATE TABLE ProveedorUbicacion (
    id_ubicacionProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    direccion VARCHAR(100),
    localidad VARCHAR(50),
    provincia VARCHAR(50),
    tipo VARCHAR(20),
    CONSTRAINT FK_ProveedorUbicacion_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.ProductoProveedor','U') IS NULL
BEGIN
CREATE TABLE ProductoProveedor (
    id_productoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_proveedor INT,
    precioCompra DECIMAL(18,2),
    tiempoEntrega INT,
    descuento DECIMAL(5,2),
    CONSTRAINT FK_ProductoProveedor_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_ProductoProveedor_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

-- ============================================
-- COMPRAS
-- ============================================
IF OBJECT_ID('dbo.Compras','U') IS NULL
BEGIN
CREATE TABLE Compras (
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    tipoDocumento VARCHAR(30),
    numeroDocumento VARCHAR(20),
    montoTotal DECIMAL(18,2),
    id_estadoCompras INT,
    CONSTRAINT FK_Compras_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    CONSTRAINT FK_Compras_EstadoCompras FOREIGN KEY (id_estadoCompras) REFERENCES EstadoCompras(id_estadoCompras)
);
END
GO

IF OBJECT_ID('dbo.DetalleCompras','U') IS NULL
BEGIN
CREATE TABLE DetalleCompras (
    id_detalleCompra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    CONSTRAINT FK_DetalleCompras_Compras FOREIGN KEY (id_compra) REFERENCES Compras(id_compra),
    CONSTRAINT FK_DetalleCompras_Productos FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.DevolucionesProveedor','U') IS NULL
BEGIN
CREATE TABLE DevolucionesProveedor (
    id_devolucion INT PRIMARY KEY IDENTITY(1,1),
    id_detalleCompra INT,
    motivo VARCHAR(100),
    fecha DATE DEFAULT GETDATE(),
    CONSTRAINT FK_Devoluciones_DetalleCompras FOREIGN KEY (id_detalleCompra) REFERENCES DetalleCompras(id_detalleCompra)
);
END
GO

-- ============================================
-- MISSING TABLES FOR PURCHASING MODULE
-- ============================================

IF OBJECT_ID('dbo.PresupuestoCompra','U') IS NULL
BEGIN
CREATE TABLE PresupuestoCompra (
    id_presupuesto INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    total DECIMAL(18,2),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.OrdenCompra','U') IS NULL
BEGIN
CREATE TABLE OrdenCompra (
    id_ordenCompra INT PRIMARY KEY IDENTITY(1,1),
    id_presupuesto INT,
    id_proveedor INT,
    fecha DATE,
    total DECIMAL(18,2),
    entregado BIT DEFAULT 0,
    FOREIGN KEY (id_presupuesto) REFERENCES PresupuestoCompra(id_presupuesto),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.DetallePresupuestoCompra','U') IS NULL
BEGIN
CREATE TABLE DetallePresupuestoCompra (
    id_detallePresupuesto INT PRIMARY KEY IDENTITY(1,1),
    id_presupuesto INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    subtotal AS (cantidad * precioUnitario) PERSISTED,
    FOREIGN KEY (id_presupuesto) REFERENCES PresupuestoCompra(id_presupuesto),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.DetalleOrdenCompra','U') IS NULL
BEGIN
CREATE TABLE DetalleOrdenCompra (
    id_detalleOrden INT PRIMARY KEY IDENTITY(1,1),
    id_ordenCompra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    subtotal AS (cantidad * precioUnitario) PERSISTED,
    recibidoCantidad INT NULL,
    FOREIGN KEY (id_ordenCompra) REFERENCES OrdenCompra(id_ordenCompra),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.Remito','U') IS NULL
BEGIN
CREATE TABLE Remito (
    id_remito INT PRIMARY KEY IDENTITY(1,1),
    id_ordenCompra INT,
    numeroRemito VARCHAR(50),
    fecha DATE,
    conFactura BIT,
    visadoAlmacen BIT DEFAULT 0,
    FOREIGN KEY (id_ordenCompra) REFERENCES OrdenCompra(id_ordenCompra)
);
END
GO

IF OBJECT_ID('dbo.FacturaCompra','U') IS NULL
BEGIN
CREATE TABLE FacturaCompra (
    id_factura INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    id_remito INT,
    numeroFactura VARCHAR(50),
    fecha DATE,
    total DECIMAL(18,2),
    visadoAlmacen BIT,
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_remito) REFERENCES Remito(id_remito)
);
END
GO

IF OBJECT_ID('dbo.NotaCredito','U') IS NULL
BEGIN
CREATE TABLE NotaCredito (
    id_notaCredito INT PRIMARY KEY IDENTITY(1,1),
    id_factura INT,
    fecha DATE,
    monto DECIMAL(18,2),
    motivo VARCHAR(255),
    FOREIGN KEY (id_factura) REFERENCES FacturaCompra(id_factura)
);
END
GO

IF OBJECT_ID('dbo.NotaDebito','U') IS NULL
BEGIN
CREATE TABLE NotaDebito (
    id_notaDebito INT PRIMARY KEY IDENTITY(1,1),
    id_factura INT,
    fecha DATE,
    monto DECIMAL(18,2),
    motivo VARCHAR(255),
    FOREIGN KEY (id_factura) REFERENCES FacturaCompra(id_factura)
);
END
GO

IF OBJECT_ID('dbo.DetalleFacturaCompra','U') IS NULL
BEGIN
CREATE TABLE DetalleFacturaCompra (
    id_detalleFactura INT PRIMARY KEY IDENTITY(1,1),
    id_factura INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    FOREIGN KEY (id_factura) REFERENCES FacturaCompra(id_factura),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

-- ============================================
-- CLIENTES
-- ============================================
IF OBJECT_ID('dbo.Clientes','U') IS NULL
BEGIN
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT_DNI VARCHAR(20),
    id_formaPago INT,
    limiteCredito DECIMAL(18,2),
    descuento DECIMAL(5,2),
    estado VARCHAR(45),
    CONSTRAINT FK_Clientes_FormaPago FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);
END
GO

IF OBJECT_ID('dbo.ClienteContactos','U') IS NULL
BEGIN
CREATE TABLE ClienteContactos (
    id_telefonoCliente INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(20),
    email VARCHAR(100),
    CONSTRAINT FK_ClienteContactos_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);
END
GO

IF OBJECT_ID('dbo.ClienteDirecciones','U') IS NULL
BEGIN
CREATE TABLE ClienteDirecciones (
    id_direccion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    direccion VARCHAR(100),
    localidad VARCHAR(45),
    provincia VARCHAR(45),
    tipo VARCHAR(20),
    CONSTRAINT FK_ClienteDirecciones_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);
END
GO

-- ============================================
-- VENTAS
-- ============================================
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    fecha DATE,
    tipoDocumento VARCHAR(50),
    numeroDocumento VARCHAR(50),
    montoTotal DECIMAL(18,2),
    id_estadoVentas INT,
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    CONSTRAINT FK_Ventas_EstadoVentas FOREIGN KEY (id_estadoVentas) REFERENCES EstadoVentas(id_estadoVentas)
);
END
GO

IF OBJECT_ID('dbo.DetalleVentas','U') IS NULL
BEGIN
CREATE TABLE DetalleVentas (
    id_detalleVentas INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    CONSTRAINT FK_DetalleVentas_Productos FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

-- ============================================
-- STOCK Y MOVIMIENTOS
-- ============================================
IF OBJECT_ID('dbo.MovimientosStock','U') IS NULL
BEGIN
CREATE TABLE MovimientosStock (
    id_movimientosStock INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT,
    tipoMovimiento VARCHAR(50),
    fecha DATE,
    id_usuario INT,
    id_producto INT,
    CONSTRAINT FK_MovStock_Usuarios FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_MovStock_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.Stock','U') IS NULL
BEGIN
CREATE TABLE Stock (
    id_stock INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_usuario INT,
    lote VARCHAR(50),
    stock INT,
    stockMinimo INT,
    stockIdeal INT,
    stockMaximo INT,
    tipoStock VARCHAR(20),
    puntoReposicion INT,
    fechaVencimiento DATE,
    estadoHabilitaciones VARCHAR(50),
    id_movimientosStock INT,
    CONSTRAINT FK_Stock_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_Stock_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_Stock_MovStock FOREIGN KEY (id_movimientosStock) REFERENCES MovimientosStock(id_movimientosStock)
);
END
GO

-- ============================================
-- SCRAP / BAJAS DE STOCK
-- ============================================
IF OBJECT_ID('dbo.ScrapProducto','U') IS NULL
BEGIN
CREATE TABLE ScrapProducto (
    id_scrapProducto INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL, 
    cantidad INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    id_motivoScrap INT NOT NULL,
    CONSTRAINT FK_Scrap_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_Scrap_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_Scrap_Motivo FOREIGN KEY (id_motivoScrap) REFERENCES MotivoScrap(id_motivoScrap)
);
END
GO

-- Vistas para compatibilidad con nombres usados en procedures (no modificamos logic)
IF OBJECT_ID('dbo.Producto','V') IS NOT NULL DROP VIEW dbo.Producto;
GO
-- View removed as it conflicts with potential table usage or is unnecessary if SPs are updated
-- CREATE VIEW dbo.Producto ...

IF OBJECT_ID('dbo.Proveedor','V') IS NOT NULL DROP VIEW dbo.Proveedor;
GO
-- View removed

-- FacturaCompra y DetalleFacturaCompra son nombres que aparecen en SPs; mapearlos a Compras/DetalleCompras
-- IF OBJECT_ID('dbo.FacturaCompra','V') IS NOT NULL DROP VIEW dbo.FacturaCompra;
-- GO
-- View removed because FacturaCompra is now a real table

-- IF OBJECT_ID('dbo.DetalleFacturaCompra','V') IS NOT NULL DROP VIEW dbo.DetalleFacturaCompra;
-- GO
-- View removed because DetalleFacturaCompra is now a real table

-- ProductoProveedor alias (por si los SPs usan Producto/Proveedor)
-- IF OBJECT_ID('dbo.ProductoProveedor_V','V') IS NOT NULL DROP VIEW dbo.ProductoProveedor_V;
-- GO
-- View removed

-- ============================================
-- Stored Procedures for AgileStockPro
-- Consolidated from individual text files

-- =============================================
-- sp_Almacenes y Stock y Scrap
-- =============================================
CREATE PROCEDURE sp_IngresoMercaderia
    @id_producto INT,
    @id_usuario INT,
    @lote VARCHAR(50),
    @cantidad INT,
    @stockMinimo INT,
    @stockIdeal INT,
    @stockMaximo INT,
    @tipoStock VARCHAR(20),
    @puntoReposicion INT,
    @fechaVencimiento DATE,
    @estadoHabilitaciones VARCHAR(50),
    @id_movimientosStock INT
AS
BEGIN
    INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
    VALUES (@id_producto, @id_usuario, @lote, @cantidad, @stockMinimo, @stockIdeal, @stockMaximo, @tipoStock, @puntoReposicion, @fechaVencimiento, @estadoHabilitaciones, @id_movimientosStock)
END
GO

CREATE PROCEDURE sp_ReporteMovimientosStock
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT ms.id_movimientosStock,
           ms.fecha,
           u.usuario AS Usuario,
           p.nombre AS Producto,
           ms.tipoMovimiento,
           ms.cantidad
    FROM MovimientosStock ms
    INNER JOIN Usuarios u ON ms.id_usuario = u.id_usuario
    INNER JOIN Productos p ON ms.id_producto = p.id_producto
    WHERE ms.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

CREATE PROCEDURE sp_MoverStockAScrap
    @id_producto INT,
    @cantidad INT,
    @id_usuario INT,
    @id_motivoScrap INT
AS
BEGIN
    INSERT INTO ScrapProducto (id_producto, cantidad, id_usuario, id_motivoScrap)
    VALUES (@id_producto, @cantidad, @id_usuario, @id_motivoScrap)

    UPDATE Stock
    SET stock = stock - @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_ReporteScrap
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT sp.id_scrapProducto,
           sp.fecha,
           u.usuario AS Usuario,
           p.nombre AS Producto,
           ms.descripcion AS Motivo,
           sp.cantidad
    FROM ScrapProducto sp
    INNER JOIN Usuarios u ON sp.id_usuario = u.id_usuario
    INNER JOIN Productos p ON sp.id_producto = p.id_producto
    INNER JOIN MotivoScrap ms ON sp.id_motivoScrap = ms.id_motivoScrap
    WHERE sp.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

-- =============================================
-- sp_clientes
-- =============================================
CREATE PROCEDURE sp_AgregarCliente
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuitDni VARCHAR(20),
    @id_formaPago INT,
    @limiteCredito DECIMAL(18,2),
    @descuento DECIMAL(5,2),
    @estado VARCHAR(45)
AS
BEGIN
    INSERT INTO Clientes (codigo, nombre, razonSocial, CUIT_DNI, id_formaPago, limiteCredito, descuento, estado)
    VALUES (@codigo, @nombre, @razonSocial, @cuitDni, @id_formaPago, @limiteCredito, @descuento, @estado)
END
GO

CREATE PROCEDURE sp_ModificarCliente
    @id_cliente INT,
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuitDni VARCHAR(20),
    @id_formaPago INT,
    @limiteCredito DECIMAL(18,2),
    @descuento DECIMAL(5,2),
    @estado VARCHAR(45)
AS
BEGIN
    UPDATE Clientes
    SET codigo = @codigo,
        nombre = @nombre,
        razonSocial = @razonSocial,
        CUIT_DNI = @cuitDni,
        id_formaPago = @id_formaPago,
        limiteCredito = @limiteCredito,
        descuento = @descuento,
        estado = @estado
    WHERE id_cliente = @id_cliente
END
GO

CREATE PROCEDURE sp_EliminarCliente
    @id_cliente INT
AS
BEGIN
    DELETE FROM Clientes WHERE id_cliente = @id_cliente
END
GO

CREATE PROCEDURE sp_ConsultarClientePorNombre
    @nombre VARCHAR(50)
AS
BEGIN
    SELECT 
        id_cliente,
        codigo,
        nombre,
        razonSocial,
        CUIT_DNI,
        id_formaPago,
        limiteCredito,
        descuento,
        estado
    FROM Clientes
    WHERE nombre LIKE '%' + @nombre + '%' OR razonSocial LIKE '%' + @nombre + '%'
END
GO

CREATE PROCEDURE sp_ConsultarClientePorCUIT
    @cuitDni VARCHAR(20)
AS
BEGIN
    SELECT 
        id_cliente,
        codigo,
        nombre,
        razonSocial,
        CUIT_DNI,
        id_formaPago,
        limiteCredito,
        descuento,
        estado
    FROM Clientes
    WHERE CUIT_DNI LIKE '%' + @cuitDni + '%'
END
GO

CREATE PROCEDURE sp_AgregarTelefonoCliente
    @id_cliente INT,
    @telefono VARCHAR(20),
    @sector VARCHAR(50),
    @horario VARCHAR(20),
    @email VARCHAR(100)
AS
BEGIN
    INSERT INTO ClienteContactos (id_cliente, telefono, sector, horario, email)
    VALUES (@id_cliente, @telefono, @sector, @horario, @email)
END
GO

CREATE PROCEDURE sp_AgregarDireccionCliente
    @id_cliente INT,
    @direccion VARCHAR(100),
    @localidad VARCHAR(45),
    @provincia VARCHAR(45),
    @tipo VARCHAR(20)
AS
BEGIN
    INSERT INTO ClienteDirecciones (id_cliente, direccion, localidad, provincia, tipo)
    VALUES (@id_cliente, @direccion, @localidad, @provincia, @tipo)
END
GO

-- =============================================
-- Sp_compras y estados contables
-- =============================================
CREATE PROCEDURE sp_RegistrarPresupuestoCompra
    @id_proveedor INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO PresupuestoCompra (id_proveedor, fecha, total)
    VALUES (@id_proveedor, @fecha, @total);
    SELECT SCOPE_IDENTITY() AS id_presupuesto;
END
GO

CREATE PROCEDURE sp_AgregarItemPresupuestoCompra
    @id_presupuesto INT,
    @id_producto INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2)
AS
BEGIN
    INSERT INTO DetallePresupuestoCompra (id_presupuesto, id_producto, cantidad, precioUnitario)
    VALUES (@id_presupuesto, @id_producto, @cantidad, @precioUnitario);
    UPDATE PresupuestoCompra SET total = (
        SELECT ISNULL(SUM(cantidad * precioUnitario),0) FROM DetallePresupuestoCompra WHERE id_presupuesto = @id_presupuesto
    ) WHERE id_presupuesto = @id_presupuesto;
END
GO

CREATE PROCEDURE sp_RegistrarOrdenCompra
    @id_presupuesto INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO OrdenCompra (id_presupuesto, id_proveedor, fecha, total)
    SELECT p.id_presupuesto, p.id_proveedor, @fecha, @total FROM PresupuestoCompra p WHERE p.id_presupuesto = @id_presupuesto;
END
GO

CREATE PROCEDURE sp_AgregarItemOrdenCompra
    @id_ordenCompra INT,
    @id_producto INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2)
AS
BEGIN
    INSERT INTO DetalleOrdenCompra (id_ordenCompra, id_producto, cantidad, precioUnitario)
    VALUES (@id_ordenCompra, @id_producto, @cantidad, @precioUnitario);
    UPDATE OrdenCompra SET total = (
        SELECT ISNULL(SUM(cantidad * precioUnitario),0) FROM DetalleOrdenCompra WHERE id_ordenCompra = @id_ordenCompra
    ) WHERE id_ordenCompra = @id_ordenCompra;
END
GO

CREATE PROCEDURE sp_ConvertirPresupuestoAOrden
    @id_presupuesto INT,
    @fecha DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @fecha IS NULL SET @fecha = GETDATE();
    DECLARE @total DECIMAL(18,2);
    SELECT @total = ISNULL(SUM(cantidad * precioUnitario),0) FROM DetallePresupuestoCompra WHERE id_presupuesto = @id_presupuesto;
    INSERT INTO OrdenCompra (id_presupuesto, id_proveedor, fecha, total)
    SELECT p.id_presupuesto, p.id_proveedor, @fecha, @total FROM PresupuestoCompra p WHERE p.id_presupuesto = @id_presupuesto;
    DECLARE @newOrdenId INT = SCOPE_IDENTITY();
    INSERT INTO DetalleOrdenCompra (id_ordenCompra, id_producto, cantidad, precioUnitario)
    SELECT @newOrdenId, id_producto, cantidad, precioUnitario FROM DetallePresupuestoCompra WHERE id_presupuesto = @id_presupuesto;
    SELECT @newOrdenId AS id_ordenCompra;
END
GO

CREATE PROCEDURE sp_ListarPresupuestosCompra
    @id_proveedor INT = NULL,
    @fechaDesde DATE = NULL,
    @fechaHasta DATE = NULL
AS
BEGIN
    SELECT p.id_presupuesto,
        p.id_proveedor,
        pr.nombre AS proveedorNombre,
        pr.CUIT AS proveedorCUIT,
        p.fecha,
        p.total,
        (SELECT COUNT(*) FROM DetallePresupuestoCompra d WHERE d.id_presupuesto = p.id_presupuesto) AS items
    FROM PresupuestoCompra p
    LEFT JOIN Proveedores pr ON pr.id_proveedor = p.id_proveedor
    WHERE (@id_proveedor IS NULL OR p.id_proveedor = @id_proveedor)
      AND (@fechaDesde IS NULL OR p.fecha >= @fechaDesde)
      AND (@fechaHasta IS NULL OR p.fecha <= @fechaHasta)
    ORDER BY p.fecha DESC;
END
GO

CREATE PROCEDURE sp_ListarOrdenesCompra
    @id_proveedor INT = NULL,
    @entregado BIT = NULL
AS
BEGIN
    SELECT o.id_ordenCompra,
        o.id_presupuesto,
        o.id_proveedor,
        pr.nombre AS proveedorNombre,
        pr.CUIT AS proveedorCUIT,
        o.fecha,
        o.total,
        o.entregado,
        (SELECT COUNT(*) FROM DetalleOrdenCompra d WHERE d.id_ordenCompra = o.id_ordenCompra) AS items
    FROM OrdenCompra o
    LEFT JOIN Proveedores pr ON pr.id_proveedor = o.id_proveedor
    WHERE (@id_proveedor IS NULL OR o.id_proveedor = @id_proveedor)
      AND (@entregado IS NULL OR o.entregado = @entregado)
    ORDER BY o.fecha DESC;
END
GO

CREATE PROCEDURE sp_MarcarOrdenCompraRecibida
    @id_ordenCompra INT
AS
BEGIN
    UPDATE OrdenCompra SET entregado = 1 WHERE id_ordenCompra = @id_ordenCompra;
END
GO
CREATE PROCEDURE sp_ListarItemsPresupuestoCompra
    @id_presupuesto INT
AS
BEGIN
    SELECT d.id_detallePresupuesto AS id_detalle,
           d.id_presupuesto,
           d.id_producto,
           d.cantidad,
           d.precioUnitario,
           d.subtotal
    FROM DetallePresupuestoCompra d
    WHERE d.id_presupuesto = @id_presupuesto
    ORDER BY d.id_detallePresupuesto ASC;
END
GO
CREATE PROCEDURE sp_ListarItemsOrdenCompra
    @id_ordenCompra INT
AS
BEGIN
    SELECT d.id_detalleOrden AS id_detalle,
           d.id_ordenCompra,
           d.id_producto,
           d.cantidad,
           d.precioUnitario,
           d.recibidoCantidad,
           d.subtotal
    FROM DetalleOrdenCompra d
    WHERE d.id_ordenCompra = @id_ordenCompra
    ORDER BY d.id_detalleOrden ASC;
END
GO

CREATE PROCEDURE sp_ConsultarPedidos
    @entregado BIT = NULL
AS
BEGIN
    SELECT 
        oc.id_ordenCompra,
        p.razonSocial AS proveedor,
        oc.fecha,
        oc.total,
        oc.entregado
    FROM OrdenCompra oc
    INNER JOIN Proveedores p ON oc.id_proveedor = p.id_proveedor
    WHERE (@entregado IS NULL OR oc.entregado = @entregado)
END
GO

CREATE PROCEDURE sp_RegistrarRemito
    @id_ordenCompra INT,
    @numeroRemito VARCHAR(50),
    @fecha DATE,
    @conFactura BIT
AS
BEGIN
    INSERT INTO Remito (id_ordenCompra, numeroRemito, fecha, conFactura)
    VALUES (@id_ordenCompra, @numeroRemito, @fecha, @conFactura)
END
GO

CREATE PROCEDURE sp_RegistrarFacturaCompra
    @id_proveedor INT,
    @id_remito INT,
    @numeroFactura VARCHAR(50),
    @fecha DATE,
    @total DECIMAL(18,2),
    @visadoAlmacen BIT
AS
BEGIN
    INSERT INTO FacturaCompra (id_proveedor, id_remito, numeroFactura, fecha, total, visadoAlmacen)
    VALUES (@id_proveedor, @id_remito, @numeroFactura, @fecha, @total, @visadoAlmacen)
END
GO

CREATE PROCEDURE sp_RegistrarNotaCredito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    INSERT INTO NotaCredito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

CREATE PROCEDURE sp_RegistrarNotaDebito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    INSERT INTO NotaDebito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

CREATE PROCEDURE sp_ReporteCompras
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL,
    @id_proveedor INT = NULL,
    @id_producto INT = NULL
AS
BEGIN
    SELECT 
        fc.id_factura,
        p.razonSocial AS proveedor,
        pr.nombre AS producto,
        fc.fecha,
        fc.total
    FROM FacturaCompra fc
    INNER JOIN Proveedores p ON fc.id_proveedor = p.id_proveedor
    INNER JOIN DetalleFacturaCompra df ON fc.id_factura = df.id_factura
    INNER JOIN Productos pr ON df.id_producto = pr.id_producto
    WHERE 
        (@fechaInicio IS NULL OR fc.fecha >= @fechaInicio) AND
        (@fechaFin IS NULL OR fc.fecha <= @fechaFin) AND
        (@id_proveedor IS NULL OR fc.id_proveedor = @id_proveedor) AND
        (@id_producto IS NULL OR pr.id_producto = @id_producto)
END
GO

CREATE PROCEDURE sp_ReporteDevolucionesProveedores
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
AS
BEGIN
    SELECT 
        d.id_devolucion,
        p.razonSocial AS proveedor,
        pr.nombre AS producto,
        d.fecha,
        d.motivo
    FROM DevolucionesProveedor d
    INNER JOIN DetalleCompras dc ON d.id_detalleCompra = dc.id_detalleCompra
    INNER JOIN Compras c ON dc.id_compra = c.id_compra
    INNER JOIN Proveedores p ON c.id_proveedor = p.id_proveedor
    INNER JOIN Productos pr ON dc.id_producto = pr.id_producto
    WHERE 
        (@fechaInicio IS NULL OR d.fecha >= @fechaInicio) AND
        (@fechaFin IS NULL OR d.fecha <= @fechaFin)
END
GO

-- =============================================
-- Sp_proveedores
-- =============================================
CREATE PROCEDURE sp_AgregarProveedor
    @codigo VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuit VARCHAR(20),
    @email VARCHAR(100),
    @formaPago VARCHAR(100),
    @tiempoEntrega VARCHAR(50),
    @descuento DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Proveedores (codigo, razonSocial, CUIT, email, formaPago, TiempoEntrega, Descuento)
    VALUES (@codigo, @razonSocial, @cuit, @email, @formaPago, CAST(@tiempoEntrega AS INT), @descuento)
END
GO

CREATE PROCEDURE sp_ModificarProveedor
    @id_proveedor INT,
    @codigo VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuit VARCHAR(20),
    @email VARCHAR(100),
    @formaPago VARCHAR(100),
    @tiempoEntrega VARCHAR(50),
    @descuento DECIMAL(5,2)
AS
BEGIN
    UPDATE Proveedores
    SET codigo = @codigo,
        razonSocial = @razonSocial,
        CUIT = @cuit,
        email = @email,
        formaPago = @formaPago,
        TiempoEntrega = CAST(@tiempoEntrega AS INT),
        Descuento = @descuento
    WHERE id_proveedor = @id_proveedor
END
GO

CREATE PROCEDURE sp_EliminarProveedor
    @id_proveedor INT
AS
BEGIN
    DELETE FROM Proveedores WHERE id_proveedor = @id_proveedor
END
GO

CREATE PROCEDURE sp_ConsultarProveedoresPorNombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT 
        id_proveedor,
        codigo,
        razonSocial,
        CUIT,
        email,
        formaPago,
        TiempoEntrega,
        Descuento
    FROM Proveedores
    WHERE razonSocial LIKE '%' + @nombre + '%'
END
GO

CREATE PROCEDURE sp_ConsultarProveedoresPorCUIT
    @cuit VARCHAR(20)
AS
BEGIN
    SELECT 
        id_proveedor,
        codigo,
        razonSocial,
        CUIT,
        email,
        formaPago,
        TiempoEntrega,
        Descuento
    FROM Proveedores
    WHERE CUIT LIKE '%' + @cuit + '%'
END
GO

CREATE PROCEDURE sp_AgregarTelefonoProveedor
    @id_proveedor INT,
    @contacto VARCHAR(100),
    @sector VARCHAR(100),
    @telefono VARCHAR(50),
    @email VARCHAR(100),
    @horario VARCHAR(100)
AS
BEGIN
    INSERT INTO ProveedorTelefonos (id_proveedor, contacto, sector, telefono, email, horario)
    VALUES (@id_proveedor, @contacto, @sector, @telefono, @email, @horario)
END
GO
IF OBJECT_ID('dbo.ProveedorContactos','U') IS NULL
BEGIN
CREATE TABLE ProveedorContactos(
    id_contactoProveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(50),
    Nombre VARCHAR(80),
    Cargo VARCHAR(80),
    Email VARCHAR(100),
    Telefono VARCHAR(50)
);
END
GO
CREATE PROCEDURE sp_AgregarProveedorContacto
    @codigo VARCHAR(50),
    @Nombre VARCHAR(80),
    @Cargo VARCHAR(80),
    @Email VARCHAR(100),
    @Telefono VARCHAR(50)
AS
BEGIN
    INSERT INTO ProveedorContactos(codigo,Nombre,Cargo,Email,Telefono)
    VALUES(@codigo,@Nombre,@Cargo,@Email,@Telefono)
END
GO

CREATE PROCEDURE sp_AgregarDireccionProveedor
    @id_proveedor INT,
    @direccion VARCHAR(255),
    @localidad VARCHAR(100),
    @provincia VARCHAR(100)
AS
BEGIN
    INSERT INTO ProveedorUbicacion (id_proveedor, direccion, localidad, provincia, tipo)
    VALUES (@id_proveedor, @direccion, @localidad, @provincia, 'Principal')
END
GO

CREATE PROCEDURE sp_RelacionarProductoProveedor
    @id_proveedor INT,
    @id_producto INT,
    @precioCompra DECIMAL(18,2) = NULL,
    @tiempoEntrega INT = NULL,
    @descuento DECIMAL(5,2) = NULL
AS
BEGIN
    INSERT INTO ProductoProveedor (id_proveedor, id_producto, precioCompra, tiempoEntrega, descuento)
    VALUES (@id_proveedor, @id_producto, @precioCompra, @tiempoEntrega, @descuento)
END
GO

CREATE PROCEDURE sp_ConsultarProductosPorProveedor
    @id_proveedor INT
AS
BEGIN
    SELECT 
        p.id_producto,
        p.codigo,
        p.nombre,
        p.descripcion,
        pp.precioCompra,
        pp.tiempoEntrega,
        pp.descuento
    FROM ProductoProveedor pp
    INNER JOIN Productos p ON pp.id_producto = p.id_producto
    WHERE pp.id_proveedor = @id_proveedor
END
GO

CREATE PROCEDURE sp_ConsultarProveedoresPorProducto
    @id_producto INT
AS
BEGIN
    SELECT 
        pr.id_proveedor,
        pr.razonSocial,
        pr.CUIT,
        pr.formaPago,
        pp.precioCompra,
        pp.tiempoEntrega,
        pp.descuento
    FROM ProductoProveedor pp
    INNER JOIN Proveedores pr ON pp.id_proveedor = pr.id_proveedor
    WHERE pp.id_producto = @id_producto
END
GO
CREATE PROCEDURE sp_DeleteRelacionProductoProveedor
    @id_producto INT,
    @id_proveedor INT
AS
BEGIN
    DELETE FROM ProductoProveedor WHERE id_producto = @id_producto AND id_proveedor = @id_proveedor;
END
GO

CREATE PROCEDURE sp_ActualizarRelacionProductoProveedor
    @id_producto INT,
    @id_proveedor INT,
    @precioCompra DECIMAL(18,2) = NULL,
    @tiempoEntrega INT = NULL,
    @descuento DECIMAL(5,2) = NULL
AS
BEGIN
    UPDATE ProductoProveedor SET
        precioCompra = @precioCompra,
        tiempoEntrega = @tiempoEntrega,
        descuento = @descuento
    WHERE id_producto = @id_producto AND id_proveedor = @id_proveedor;
END
GO

-- =============================================
-- Sp_productos
-- =============================================
CREATE PROCEDURE sp_AgregarProducto
    @codigo VARCHAR(50),
    @codBarras VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255),
    @id_marca INT,
    @precioCompra DECIMAL(18,2),
    @precioVenta DECIMAL(18,2),
    @estado VARCHAR(50),
    @ubicacion VARCHAR(100),
    @habilitado BIT,
    @id_categoria INT,
    @unidadMedida VARCHAR(20) = NULL,
    @peso DECIMAL(18,2) = NULL,
    @volumen DECIMAL(18,2) = NULL,
    @puntoReposicion INT = NULL,
    @diasVencimiento INT = NULL,
    @loteObligatorio BIT = NULL,
    @controlVencimiento BIT = NULL
AS
BEGIN
    INSERT INTO Productos (codigo, codBarras, nombre, descripcion, id_marca, precioCompra, precioVenta, estado, ubicacion, habilitado, id_categoria, unidadMedida, peso, volumen, puntoReposicion, diasVencimiento, loteObligatorio, controlVencimiento)
    VALUES (@codigo, @codBarras, @nombre, @descripcion, @id_marca, @precioCompra, @precioVenta, @estado, @ubicacion, @habilitado, @id_categoria, @unidadMedida, @peso, @volumen, @puntoReposicion, @diasVencimiento, @loteObligatorio, @controlVencimiento)
END
GO

CREATE PROCEDURE sp_ModificarProducto
    @id_producto INT,
    @codigo VARCHAR(50),
    @codBarras VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255),
    @id_marca INT,
    @precioCompra DECIMAL(18,2),
    @precioVenta DECIMAL(18,2),
    @estado VARCHAR(50),
    @ubicacion VARCHAR(100),
    @habilitado BIT,
    @id_categoria INT,
    @unidadMedida VARCHAR(20) = NULL,
    @peso DECIMAL(18,2) = NULL,
    @volumen DECIMAL(18,2) = NULL,
    @puntoReposicion INT = NULL,
    @diasVencimiento INT = NULL,
    @loteObligatorio BIT = NULL,
    @controlVencimiento BIT = NULL
AS
BEGIN
    UPDATE Productos
    SET codigo = @codigo,
        codBarras = @codBarras,
        nombre = @nombre,
        descripcion = @descripcion,
        id_marca = @id_marca,
        precioCompra = @precioCompra,
        precioVenta = @precioVenta,
        estado = @estado,
        ubicacion = @ubicacion,
        habilitado = @habilitado,
        id_categoria = @id_categoria,
        unidadMedida = @unidadMedida,
        peso = @peso,
        volumen = @volumen,
        puntoReposicion = @puntoReposicion,
        diasVencimiento = @diasVencimiento,
        loteObligatorio = @loteObligatorio,
        controlVencimiento = @controlVencimiento
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_EliminarProducto
    @id_producto INT
AS
BEGIN
    DELETE FROM Productos WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SELECT p.id_producto, p.codigo, p.nombre, p.descripcion, m.marca AS Marca, c.categoria AS Categoria,
           p.precioCompra, p.precioVenta, p.estado, p.ubicacion, p.habilitado
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
END
GO

CREATE PROCEDURE sp_BuscarProducto
    @busqueda VARCHAR(100)
AS
BEGIN
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @busqueda + '%'
       OR p.codigo LIKE '%' + @busqueda + '%'
       OR m.marca LIKE '%' + @busqueda + '%'
       OR c.categoria LIKE '%' + @busqueda + '%'
END
GO

CREATE PROCEDURE sp_ProductosHabilitados
AS
BEGIN
    SELECT * FROM Productos WHERE habilitado = 1
END
GO

CREATE PROCEDURE sp_ProductosPorCategoria
    @id_categoria INT
AS
BEGIN
    SELECT * FROM Productos WHERE id_categoria = @id_categoria
END
GO

CREATE PROCEDURE sp_ProductosPorMarca
    @id_marca INT
AS
BEGIN
    SELECT * FROM Productos WHERE id_marca = @id_marca
END
GO

CREATE PROCEDURE sp_ConsultarProductoPorNombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @nombre + '%'
END
GO

CREATE PROCEDURE sp_ConsultarProductoPorCodigo
    @codigo VARCHAR(50)
AS
BEGIN
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.codigo = @codigo
END
GO

-- =============================================
-- sp_tablas auxiliares
-- =============================================
-- CategorÃ­as Producto
CREATE PROCEDURE sp_AgregarCategoria
    @categoria VARCHAR(100),
    @descripcion VARCHAR(100)
AS
BEGIN
    INSERT INTO CategoriasProducto (categoria, descripcion)
    VALUES (@categoria, @descripcion)
END
GO

CREATE PROCEDURE sp_ModificarCategoria
    @id_categoria INT,
    @categoria VARCHAR(100),
    @descripcion VARCHAR(100)
AS
BEGIN
    UPDATE CategoriasProducto
    SET categoria = @categoria,
        descripcion = @descripcion
    WHERE id_categoria = @id_categoria
END
GO

CREATE PROCEDURE sp_EliminarCategoria
    @id_categoria INT
AS
BEGIN
    DELETE FROM CategoriasProducto
    WHERE id_categoria = @id_categoria
END
GO

-- Marcas Producto
CREATE PROCEDURE sp_AgregarMarca
    @marca VARCHAR(45)
AS
BEGIN
    INSERT INTO MarcasProducto (marca)
    VALUES (@marca)
END
GO

CREATE PROCEDURE sp_ModificarMarca
    @id_marca INT,
    @marca VARCHAR(45)
AS
BEGIN
    UPDATE MarcasProducto
    SET marca = @marca
    WHERE id_marca = @id_marca
END
GO

CREATE PROCEDURE sp_EliminarMarca
    @id_marca INT
AS
BEGIN
    DELETE FROM MarcasProducto
    WHERE id_marca = @id_marca
END
GO

-- Formas de Pago
CREATE PROCEDURE sp_AgregarFormaPago
    @descripcion VARCHAR(100)
AS
BEGIN
    INSERT INTO FormaPago (descripcion)
    VALUES (@descripcion)
END
GO

CREATE PROCEDURE sp_ModificarFormaPago
    @id_formaPago INT,
    @descripcion VARCHAR(100)
AS
BEGIN
    UPDATE FormaPago
    SET descripcion = @descripcion
    WHERE id_formaPago = @id_formaPago
END
GO

CREATE PROCEDURE sp_EliminarFormaPago
    @id_formaPago INT
AS
BEGIN
    DELETE FROM FormaPago
    WHERE id_formaPago = @id_formaPago
END
GO

-- Estados Compras
CREATE PROCEDURE sp_ModificarEstadoCompra
    @id_estadoCompras INT,
    @pendiente BIT,
    @aprobada BIT,
    @recibida BIT,
    @cancelada BIT
AS
BEGIN
    UPDATE EstadoCompras
    SET pendiente = @pendiente,
        aprobada = @aprobada,
        recibida = @recibida,
        cancelada = @cancelada
    WHERE id_estadoCompras = @id_estadoCompras
END
GO

-- Estados Ventas
CREATE PROCEDURE sp_ModificarEstadoVenta
    @id_estadoVentas INT,
    @facturada BIT,
    @entregada BIT,
    @cancelada BIT
AS
BEGIN
    UPDATE EstadoVentas
    SET facturada = @facturada,
        entregada = @entregada,
        cancelada = @cancelada
    WHERE id_estadoVentas = @id_estadoVentas
END
GO

-- =============================================
-- sp_ventas
-- =============================================
CREATE PROCEDURE sp_AgregarPresupuestoVenta
    @id_cliente INT,
    @fecha DATE,
    @tipoDocumento VARCHAR(50),
    @numeroDocumento VARCHAR(50) = NULL,
    @montoTotal DECIMAL(18,2),
    @id_estadoVentas INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Si no se pasa un estado válido, tomar el primero disponible como fallback
    IF @id_estadoVentas IS NULL OR NOT EXISTS (SELECT 1 FROM EstadoVentas WHERE id_estadoVentas = @id_estadoVentas)
    BEGIN
        SELECT TOP 1 @id_estadoVentas = id_estadoVentas FROM EstadoVentas ORDER BY id_estadoVentas;
        IF @id_estadoVentas IS NULL
        BEGIN
            RAISERROR('No existe ningún registro en EstadoVentas. Ejecutá el seed correspondiente.',16,1);
            RETURN;
        END
    END

    INSERT INTO Ventas (id_cliente, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoVentas)
    VALUES (@id_cliente, @fecha, @tipoDocumento, @numeroDocumento, @montoTotal, @id_estadoVentas);

    -- Devolver id creado para que el repositorio pueda leerlo
    SELECT SCOPE_IDENTITY() AS id_venta;
END
GO

CREATE PROCEDURE sp_AgregarNotaPedido
    @id_venta INT,
    @id_producto INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2),
    @subtotal DECIMAL(18,2)
AS
BEGIN
    INSERT INTO DetalleVentas (id_venta, id_producto, cantidad, precioUnitario, subtotal)
    VALUES (@id_venta, @id_producto, @cantidad, @precioUnitario, @subtotal)
    
    UPDATE Stock
    SET stock = stock - @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_AgregarNotaCredito
    @id_venta INT,
    @monto DECIMAL(18,2)
AS
BEGIN
    UPDATE Ventas
    SET montoTotal = montoTotal - @monto
    WHERE id_venta = @id_venta
END
GO

CREATE PROCEDURE sp_AgregarNotaDebito
    @id_venta INT,
    @monto DECIMAL(18,2)
AS
BEGIN
    UPDATE Ventas
    SET montoTotal = montoTotal + @monto
    WHERE id_venta = @id_venta
END
GO

CREATE PROCEDURE sp_DevolverProductoStock
    @id_producto INT,
    @cantidad INT
AS
BEGIN
    UPDATE Stock
    SET stock = stock + @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_ReporteVentas
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT v.id_venta,
           v.fecha,
           c.nombre AS Cliente,
           c.razonSocial AS RazonSocial,
           p.nombre AS Producto,
           cat.categoria AS Categoria,
           dv.cantidad,
           dv.precioUnitario,
           dv.subtotal,
           v.montoTotal
    FROM Ventas v
    INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
    INNER JOIN DetalleVentas dv ON v.id_venta = dv.id_venta
    INNER JOIN Productos p ON dv.id_producto = p.id_producto
    INNER JOIN CategoriasProducto cat ON p.id_categoria = cat.id_categoria
    WHERE v.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

-- =============================================
-- Helper procedures for repositories that currently use SQL directo
-- =============================================

-- Politica de seguridad (GetPoliticaSeguridadAsync)
IF OBJECT_ID('dbo.sp_get_politica_seguridad', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_politica_seguridad;
GO
CREATE PROCEDURE sp_get_politica_seguridad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 *
    FROM politicas_seguridad;
END
GO

-- Preguntas de seguridad (GetPreguntasSeguridadAsync)
IF OBJECT_ID('dbo.sp_get_preguntas_seguridad', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_preguntas_seguridad;
GO
CREATE PROCEDURE sp_get_preguntas_seguridad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_pregunta, pregunta
    FROM preguntas_seguridad;
END
GO

-- Respuestas por usuario (GetRespuestasSeguridadByUsuarioIdAsync)
IF OBJECT_ID('dbo.sp_get_respuestas_seguridad_by_usuario', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_respuestas_seguridad_by_usuario;
GO
CREATE PROCEDURE sp_get_respuestas_seguridad_by_usuario
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_usuario, id_pregunta, respuesta
    FROM respuestas_seguridad
    WHERE id_usuario = @id_usuario;
END
GO

-- Borrar respuestas por usuario (DeleteRespuestasSeguridadByUsuarioIdAsync)
IF OBJECT_ID('dbo.sp_delete_respuestas_seguridad_by_usuario', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_delete_respuestas_seguridad_by_usuario;
GO
CREATE PROCEDURE sp_delete_respuestas_seguridad_by_usuario
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM respuestas_seguridad
    WHERE id_usuario = @id_usuario;
END
GO

-- Personas: listado y detalle con joins (para SqlPersonaRepository)
IF OBJECT_ID('dbo.sp_get_persona_by_id', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_persona_by_id;
GO
CREATE PROCEDURE sp_get_persona_by_id
    @id_persona INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_persona, p.legajo, p.nombre, p.apellido, p.id_tipo_doc, p.num_doc, p.fecha_nacimiento, p.cuil,
        p.calle, p.altura, p.id_localidad, p.id_genero, p.correo, p.celular, p.fecha_ingreso,
        td.tipo_doc AS TipoDocNombre,
        l.localidad AS LocalidadNombre,
        pa.id_partido AS IdPartido,
        pa.partido AS PartidoNombre,
        pr.id_provincia AS IdProvincia,
        pr.provincia AS ProvinciaNombre,
        g.genero AS GeneroNombre
    FROM personas p
    LEFT JOIN tipo_doc td ON p.id_tipo_doc = td.id_tipo_doc
    LEFT JOIN localidades l ON p.id_localidad = l.id_localidad
    LEFT JOIN partidos pa ON l.id_partido = pa.id_partido
    LEFT JOIN provincias pr ON pa.id_provincia = pr.id_provincia
    LEFT JOIN generos g ON p.id_genero = g.id_genero
    WHERE p.id_persona = @id_persona;
END
GO

IF OBJECT_ID('dbo.sp_get_personas', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_personas;
GO
CREATE PROCEDURE sp_get_personas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_persona, p.legajo, p.nombre, p.apellido, p.id_tipo_doc, p.num_doc, p.fecha_nacimiento, p.cuil,
        p.calle, p.altura, p.id_localidad, p.id_genero, p.correo, p.celular, p.fecha_ingreso,
        td.tipo_doc AS TipoDocNombre,
        l.localidad AS LocalidadNombre,
        pa.id_partido AS IdPartido,
        pa.partido AS PartidoNombre,
        pr.id_provincia AS IdProvincia,
        pr.provincia AS ProvinciaNombre,
        g.genero AS GeneroNombre
    FROM personas p
    LEFT JOIN tipo_doc td ON p.id_tipo_doc = td.id_tipo_doc
    LEFT JOIN localidades l ON p.id_localidad = l.id_localidad
    LEFT JOIN partidos pa ON l.id_partido = pa.id_partido
    LEFT JOIN provincias pr ON pa.id_provincia = pr.id_provincia
    LEFT JOIN generos g ON p.id_genero = g.id_genero;
END
GO

-- Motivos de scrap (GetScrapReasonsAsync)
IF OBJECT_ID('dbo.sp_get_motivos_scrap', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_motivos_scrap;
GO
CREATE PROCEDURE sp_get_motivos_scrap
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_motivoScrap, dano, vencido, obsoleto, malaCalidad
    FROM MotivoScrap;
END
GO

-- Stock completo (GetStockAsync)
IF OBJECT_ID('dbo.sp_get_stock', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_stock;
GO
CREATE PROCEDURE sp_get_stock
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.id_stock,
           p.nombre AS Producto,
           p.ubicacion AS Almacen,
           s.stock,
           s.stockMinimo,
           s.stockMaximo
    FROM Stock s
    INNER JOIN Productos p ON s.id_producto = p.id_producto;
END
GO

-- Catálogo: marcas habilitadas (GetBrandsAsync)
IF OBJECT_ID('dbo.sp_get_marcas', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_marcas;
GO
CREATE PROCEDURE sp_get_marcas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_marca AS Id, marca AS Nombre
    FROM MarcasProducto
    WHERE estado IS NULL OR estado = 'Habilitado';
END
GO

-- Catálogo: categorías habilitadas (GetCategoriesAsync)
IF OBJECT_ID('dbo.sp_get_categorias', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_categorias;
GO
CREATE PROCEDURE sp_get_categorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_categoria AS Id, categoria AS Nombre
    FROM CategoriasProducto
    WHERE estado IS NULL OR estado = 'Habilitado';
END
GO

-- Producto por Id con stock agregado (GetProductByIdAsync)
IF OBJECT_ID('dbo.sp_get_product_by_id_with_stock', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_product_by_id_with_stock;
GO
CREATE PROCEDURE sp_get_product_by_id_with_stock
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH StockAgg AS (
        SELECT s.id_producto,
               SUM(COALESCE(s.stock,0)) AS StockActual,
               MAX(COALESCE(s.stockMinimo,0)) AS StockMinimo,
               MAX(COALESCE(s.stockMaximo,0)) AS StockMaximo
        FROM Stock s
        GROUP BY s.id_producto
    )
        SELECT p.id_producto,
            p.codigo,
            p.nombre,
            c.categoria,
            m.marca,
            COALESCE(p.precioVenta, 0) AS precioVenta,
            COALESCE(sa.StockActual, 0) AS StockActual,
            COALESCE(sa.StockMinimo, 0) AS StockMinimo,
            COALESCE(sa.StockMaximo, 0) AS StockMaximo,
            COALESCE(p.unidadMedida,'') AS unidadMedida,
            COALESCE(p.peso,0) AS peso,
            COALESCE(p.volumen,0) AS volumen,
            COALESCE(p.puntoReposicion,0) AS puntoReposicion,
            COALESCE(p.diasVencimiento,0) AS diasVencimiento,
            CAST(COALESCE(p.loteObligatorio,0) AS BIT) AS loteObligatorio,
            CAST(COALESCE(p.controlVencimiento,0) AS BIT) AS controlVencimiento
    FROM Productos p
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN StockAgg sa ON sa.id_producto = p.id_producto
    WHERE p.id_producto = @id;
END
GO

-- Listado paginado de productos con stock (GetProductsAsync)
IF OBJECT_ID('dbo.sp_get_products_with_stock_paged', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_get_products_with_stock_paged;
GO
CREATE PROCEDURE sp_get_products_with_stock_paged
    @Search NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- CTE para productos + stock
    WITH StockAgg AS (
        SELECT s.id_producto,
               SUM(COALESCE(s.stock,0)) AS StockActual,
               MAX(COALESCE(s.stockMinimo,0)) AS StockMinimo,
               MAX(COALESCE(s.stockMaximo,0)) AS StockMaximo
        FROM Stock s
        GROUP BY s.id_producto
    ), Filtered AS (
        SELECT p.id_producto,
               p.codigo,
               p.nombre,
               c.categoria,
               m.marca,
               COALESCE(p.precioVenta, 0) AS precioVenta,
               COALESCE(sa.StockActual, 0) AS StockActual,
               COALESCE(sa.StockMinimo, 0) AS StockMinimo,
               COALESCE(sa.StockMaximo, 0) AS StockMaximo,
               COALESCE(p.unidadMedida,'') AS unidadMedida,
               COALESCE(p.peso,0) AS peso,
               COALESCE(p.volumen,0) AS volumen,
               COALESCE(p.puntoReposicion,0) AS puntoReposicion,
               COALESCE(p.diasVencimiento,0) AS diasVencimiento,
               CAST(COALESCE(p.loteObligatorio,0) AS BIT) AS loteObligatorio,
               CAST(COALESCE(p.controlVencimiento,0) AS BIT) AS controlVencimiento
        FROM Productos p
        LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
        LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
        LEFT JOIN StockAgg sa ON sa.id_producto = p.id_producto
        WHERE (@Search IS NULL OR @Search = '')
           OR p.codigo LIKE '%' + @Search + '%'
           OR p.nombre LIKE '%' + @Search + '%'
           OR c.categoria LIKE '%' + @Search + '%'
           OR m.marca LIKE '%' + @Search + '%'
    )
    SELECT *
    FROM Filtered
    ORDER BY id_producto
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Repetir CTE para el segundo SELECT (el alcance del CTE es solo la siguiente sentencia)
    WITH StockAgg AS (
        SELECT s.id_producto,
               SUM(COALESCE(s.stock,0)) AS StockActual,
               MAX(COALESCE(s.stockMinimo,0)) AS StockMinimo,
               MAX(COALESCE(s.stockMaximo,0)) AS StockMaximo
        FROM Stock s
        GROUP BY s.id_producto
    ), Filtered AS (
        SELECT p.id_producto,
               p.codigo,
               p.nombre,
               c.categoria,
               m.marca,
               COALESCE(p.precioVenta, 0) AS precioVenta,
               COALESCE(sa.StockActual, 0) AS StockActual,
               COALESCE(sa.StockMinimo, 0) AS StockMinimo,
               COALESCE(sa.StockMaximo, 0) AS StockMaximo,
               COALESCE(p.unidadMedida,'') AS unidadMedida,
               COALESCE(p.peso,0) AS peso,
               COALESCE(p.volumen,0) AS volumen,
               COALESCE(p.puntoReposicion,0) AS puntoReposicion,
               COALESCE(p.diasVencimiento,0) AS diasVencimiento,
               CAST(COALESCE(p.loteObligatorio,0) AS BIT) AS loteObligatorio,
               CAST(COALESCE(p.controlVencimiento,0) AS BIT) AS controlVencimiento
        FROM Productos p
        LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
        LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
        LEFT JOIN StockAgg sa ON sa.id_producto = p.id_producto
        WHERE (@Search IS NULL OR @Search = '')
           OR p.codigo LIKE '%' + @Search + '%'
           OR p.nombre LIKE '%' + @Search + '%'
           OR c.categoria LIKE '%' + @Search + '%'
           OR m.marca LIKE '%' + @Search + '%'
    )
    SELECT COUNT(1) AS TotalCount
    FROM Filtered;
END
GO

-- Update sp_get_users to support @RoleId filter
SET ANSI_NULLS ON
GO

-- Dashboard summary: ventas/compras de hoy, stock disponible, alertas activas
IF OBJECT_ID('dbo.sp_dashboard_summary', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_dashboard_summary;
GO
CREATE PROCEDURE dbo.sp_dashboard_summary
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @today DATE = CAST(GETDATE() AS DATE);
    DECLARE @yesterday DATE = DATEADD(DAY, -1, @today);

    -- Ventas hoy y ayer
    DECLARE @salesToday DECIMAL(18,2) = ISNULL((SELECT SUM(v.montoTotal) FROM Ventas v WHERE CAST(v.fecha AS DATE) = @today), 0);
    DECLARE @salesYesterday DECIMAL(18,2) = ISNULL((SELECT SUM(v.montoTotal) FROM Ventas v WHERE CAST(v.fecha AS DATE) = @yesterday), 0);
    DECLARE @salesDeltaPct DECIMAL(18,4) = CASE WHEN @salesYesterday = 0 THEN NULL ELSE ((@salesToday-@salesYesterday)/@salesYesterday)*100 END;

    -- Compras hoy y ayer
    DECLARE @purchToday DECIMAL(18,2) = ISNULL((SELECT SUM(f.total) FROM FacturaCompra f WHERE CAST(f.fecha AS DATE) = @today), 0);
    DECLARE @purchYesterday DECIMAL(18,2) = ISNULL((SELECT SUM(f.total) FROM FacturaCompra f WHERE CAST(f.fecha AS DATE) = @yesterday), 0);
    DECLARE @purchDeltaPct DECIMAL(18,4) = CASE WHEN @purchYesterday = 0 THEN NULL ELSE ((@purchToday-@purchYesterday)/@purchYesterday)*100 END;

    -- Stock disponible (total unidades)
    DECLARE @stockAvailable INT = ISNULL((SELECT SUM(ISNULL(s.stock,0)) FROM Stock s), 0);

    -- Alertas activas (stock bajo/crítico)
    DECLARE @alerts INT = ISNULL((
        SELECT COUNT(1)
        FROM (
            SELECT p.id_producto
            FROM Productos p
            LEFT JOIN Stock s ON p.id_producto = s.id_producto
            GROUP BY p.id_producto
            HAVING SUM(ISNULL(s.stock,0)) <= MAX(ISNULL(s.stockMinimo,0))
                OR (MAX(ISNULL(p.puntoReposicion,0)) > 0 AND SUM(ISNULL(s.stock,0)) <= MAX(ISNULL(p.puntoReposicion,0)))
        ) X
    ), 0);

    SELECT 
        -- Display strings (client expects strings)
        FORMAT(@salesToday, 'C2', 'es-AR')        AS SalesTodayDisplay,
        CASE WHEN @salesDeltaPct IS NULL THEN ''
             ELSE (CASE WHEN @salesDeltaPct>=0 THEN '+' ELSE '' END) + FORMAT(@salesDeltaPct, 'N1', 'es-AR') + '%'
        END                                       AS SalesDeltaDisplay,
        CASE WHEN @salesDeltaPct IS NULL THEN ''
             WHEN @salesDeltaPct>=0 THEN N'Subió ' + FORMAT(@salesDeltaPct, 'N1', 'es-AR') + N'% comparado a ayer'
             ELSE N'Bajó ' + FORMAT(ABS(@salesDeltaPct), 'N1', 'es-AR') + N'% comparado a ayer' END AS SalesDeltaAria,

        FORMAT(@purchToday, 'C2', 'es-AR')        AS PurchasesTodayDisplay,
        CASE WHEN @purchDeltaPct IS NULL THEN ''
             ELSE (CASE WHEN @purchDeltaPct>=0 THEN '+' ELSE '' END) + FORMAT(@purchDeltaPct, 'N1', 'es-AR') + '%'
        END                                       AS PurchasesDeltaDisplay,
        CASE WHEN @purchDeltaPct IS NULL THEN ''
             WHEN @purchDeltaPct>=0 THEN N'Subió ' + FORMAT(@purchDeltaPct, 'N1', 'es-AR') + N'% comparado a ayer'
             ELSE N'Bajó ' + FORMAT(ABS(@purchDeltaPct), 'N1', 'es-AR') + N'% comparado a ayer' END AS PurchasesDeltaAria,

        FORMAT(@stockAvailable, 'N0', 'es-AR')    AS StockAvailableDisplay,
        ''                                        AS StockDeltaDisplay,
        ''                                        AS StockDeltaAria,

        FORMAT(@alerts, 'N0', 'es-AR')            AS AlertsActiveDisplay,
        ''                                        AS AlertsDeltaDisplay,
        ''                                        AS AlertsDeltaAria;
END
GO

-- Dashboard recent activity: últimas ventas, compras y (si aplica) movimientos de stock
IF OBJECT_ID('dbo.sp_dashboard_recent', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_dashboard_recent;
GO
CREATE PROCEDURE dbo.sp_dashboard_recent
    @Top INT = 15
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @recent TABLE([When] DATETIME2, [Description] NVARCHAR(300), [Category] NVARCHAR(50));

    -- Ventas recientes
    INSERT INTO @recent([When],[Description],[Category])
    SELECT TOP(@Top)
        v.fecha,
        N'Venta #' + CONVERT(NVARCHAR(20), v.id_venta) + N' a ' + ISNULL(c.nombre,'') + N' ($' + CONVERT(NVARCHAR(50), CONVERT(DECIMAL(18,2), v.montoTotal)) + N')',
        N'sale'
    FROM Ventas v
    LEFT JOIN Clientes c ON v.id_cliente = c.id_cliente
    ORDER BY v.fecha DESC;

    -- Compras recientes
    INSERT INTO @recent([When],[Description],[Category])
    SELECT TOP(@Top)
        f.fecha,
        N'Compra ' + ISNULL(f.numeroFactura,'') + N' a ' + ISNULL(p.nombre,'') + N' ($' + CONVERT(NVARCHAR(50), CONVERT(DECIMAL(18,2), f.total)) + N')',
        N'purchase'
    FROM FacturaCompra f
    LEFT JOIN Proveedores p ON f.id_proveedor = p.id_proveedor
    ORDER BY f.fecha DESC;

    SELECT TOP(@Top)
        [When], [Description], [Category]
    FROM @recent
    ORDER BY [When] DESC;
END
GO

-- Dashboard top products: top vendidos últimos N días y stock actual
IF OBJECT_ID('dbo.sp_dashboard_top_products', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_dashboard_top_products;
GO
CREATE PROCEDURE dbo.sp_dashboard_top_products
    @Days INT = 30,
    @Top INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    WITH SalesAgg AS (
        SELECT 
            dv.id_producto,
            SUM(ISNULL(dv.cantidad,0)) AS Sold
        FROM DetalleVentas dv
        INNER JOIN Ventas v ON v.id_venta = dv.id_venta
        WHERE v.fecha >= DATEADD(DAY, -@Days, GETDATE())
        GROUP BY dv.id_producto
    ), StockAgg AS (
        SELECT s.id_producto, SUM(ISNULL(s.stock,0)) AS Stock
        FROM Stock s
        GROUP BY s.id_producto
    )
    SELECT TOP(@Top)
        p.nombre AS [Name],
        ISNULL(c.categoria,'') AS [Category],
        ISNULL(sa.Sold,0) AS [Sold],
        ISNULL(st.Stock,0) AS [Stock]
    FROM SalesAgg sa
    INNER JOIN Productos p ON sa.id_producto = p.id_producto
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    LEFT JOIN StockAgg st ON st.id_producto = p.id_producto
    ORDER BY sa.Sold DESC, p.nombre ASC;
END
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.sp_get_users', 'P') IS NULL
    EXEC('CREATE PROCEDURE dbo.sp_get_users AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE dbo.sp_get_users
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @Username NVARCHAR(30) = NULL,
    @Email NVARCHAR(100) = NULL,
    @RoleId INT = NULL,
    @SortBy NVARCHAR(100) = 'id_usuario',
    @TotalRecords INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Query NVARCHAR(MAX);
    DECLARE @CountQuery NVARCHAR(MAX);
    DECLARE @WhereClause NVARCHAR(MAX) = N'';

    IF @Username IS NOT NULL AND @Username <> ''
        SET @WhereClause = @WhereClause + ' AND u.usuario LIKE ''%'' + @Username + ''%''';

    IF @Email IS NOT NULL AND @Email <> ''
        SET @WhereClause = @WhereClause + ' AND p.correo LIKE ''%'' + @Email + ''%''';

    IF @RoleId IS NOT NULL
        SET @WhereClause = @WhereClause + ' AND u.id_rol = @RoleId';

    IF LEN(@WhereClause) > 0 SET @WhereClause = SUBSTRING(@WhereClause, 6, LEN(@WhereClause));
    ELSE SET @WhereClause = '1=1';

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
        FROM usuarios u
        INNER JOIN roles r ON u.id_rol = r.id_rol
        INNER JOIN personas p ON u.id_persona = p.id_persona
        WHERE ' + @WhereClause + N'
        ORDER BY ' + @SortBy + N'
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;';

    SET @CountQuery = N'
        SELECT @TotalRecords = COUNT(*)
        FROM usuarios u
        INNER JOIN personas p ON u.id_persona = p.id_persona
        WHERE ' + @WhereClause;

    EXEC sp_executesql @CountQuery,
        N'@Username NVARCHAR(30), @Email NVARCHAR(100), @RoleId INT, @TotalRecords INT OUTPUT',
        @Username, @Email, @RoleId, @TotalRecords OUTPUT;

    EXEC sp_executesql @Query,
        N'@PageNumber INT, @PageSize INT, @Username NVARCHAR(30), @Email NVARCHAR(100), @RoleId INT',
        @PageNumber, @PageSize, @Username, @Email, @RoleId;

END
GO
-- Seed demo Personas and Usuarios (idempotent)
SET NOCOUNT ON;

-- Declarar todas las variables al inicio
DECLARE @id_tipo_doc INT;
DECLARE @id_genero_m INT;
DECLARE @id_genero_f INT;
DECLARE @id_localidad INT;
DECLARE @rol_admin INT;
DECLARE @rol_user INT;
DECLARE @id_persona_oper INT;
DECLARE @id_persona_admin INT;
DECLARE @id_persona_admin2 INT;
DECLARE @pass VARBINARY(512);
DECLARE @pass2 VARBINARY(512);

-- Asignar valores iniciales
SELECT @id_tipo_doc = id_tipo_doc FROM tipo_doc WHERE tipo_doc = 'DNI';
SELECT @id_genero_m = id_genero FROM generos WHERE genero = 'Masculino';
SELECT @id_genero_f = id_genero FROM generos WHERE genero = 'Femenino';
SELECT @id_localidad = id_localidad FROM localidades ORDER BY id_localidad;

IF @id_tipo_doc IS NULL OR @id_genero_m IS NULL OR @id_localidad IS NULL
BEGIN
    RAISERROR('CatÃ¡logo mÃ­nimo faltante (tipo_doc/generos/localidades). Ejecuta el script de creaciÃ³n base primero.', 16, 1);
    RETURN;
END

-- Roles
IF NOT EXISTS (SELECT 1 FROM roles WHERE rol = 'Administrador') 
    EXEC sp_insert_rol @rol = 'Administrador';
IF NOT EXISTS (SELECT 1 FROM roles WHERE rol = 'Usuario') 
    EXEC sp_insert_rol @rol = 'Usuario';

SELECT @rol_admin = id_rol FROM roles WHERE rol = 'Administrador';
SELECT @rol_user = id_rol FROM roles WHERE rol = 'Usuario';

-- Persona y usuario: operador
IF NOT EXISTS (SELECT 1 FROM personas WHERE num_doc = '20000001')
BEGIN
    EXEC sp_insert_persona
        @legajo = 2,
        @nombre = 'Operador',
        @apellido = 'Demo',
        @id_tipo_doc = @id_tipo_doc,
        @num_doc = '20000001',
        @fecha_nacimiento = '1990-01-01',
        @cuil = '20200000011',
        @calle = 'Calle 1',
        @altura = '100',
        @id_localidad = @id_localidad,
        @id_genero = @id_genero_m,
        @correo = 'operador@example.com',
        @celular = '1100000001';
END

SELECT @id_persona_oper = id_persona FROM personas WHERE num_doc = '20000001';

IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'operador') AND @id_persona_oper IS NOT NULL
BEGIN
    SET @pass = 0x2F87B976666AF7E41D8C279FDAFB8BD8926B6D4A0684BB14F88A01A11E124C7B;
    DECLARE @now DATETIME = GETDATE();
    EXEC sp_insert_usuario
        @usuario = 'operador',
        @contrasena_script = @pass,
        @id_persona = @id_persona_oper,
        @fecha_bloqueo = '99991231',
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = @now,
        @id_rol = @rol_user,
        @CambioContrasenaObligatorio = 1
END

-- Persona y usuario: admin (Principal)
IF NOT EXISTS (SELECT 1 FROM personas WHERE num_doc = '20000000')
BEGIN
    EXEC sp_insert_persona
        @legajo = 1,
        @nombre = 'System',
        @apellido = 'Admin',
        @id_tipo_doc = @id_tipo_doc,
        @num_doc = '20000000',
        @fecha_nacimiento = '1980-01-01',
        @cuil = '20200000000',
        @calle = 'Calle Admin',
        @altura = '1',
        @id_localidad = @id_localidad,
        @id_genero = @id_genero_m,
        @correo = 'admin@example.com',
        @celular = '1100000000';
END

SELECT @id_persona_admin = id_persona FROM personas WHERE num_doc = '20000000';

IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'admin') AND @id_persona_admin IS NOT NULL
BEGIN
    -- Password: admin123 -> Argon2id
    SET @pass = 0xA6CD645C57030D00EB8F8CB4A2B21BBEDC54181871ACE4BB6E578D67337F4C05;
    DECLARE @now_admin DATETIME = GETDATE();
    EXEC sp_insert_usuario
        @usuario = 'admin',
        @contrasena_script = @pass,
        @id_persona = @id_persona_admin,
        @fecha_bloqueo = '99991231',
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = @now_admin,
        @id_rol = @rol_admin,
        @CambioContrasenaObligatorio = 0
END

-- Persona y usuario: admin2 (opcional, otro admin)
IF NOT EXISTS (SELECT 1 FROM personas WHERE num_doc = '20000002')
BEGIN
    EXEC sp_insert_persona
        @legajo = 3,
        @nombre = 'Admin',
        @apellido = 'Demo',
        @id_tipo_doc = @id_tipo_doc,
        @num_doc = '20000002',
        @fecha_nacimiento = '1991-02-02',
        @cuil = '20200000022',
        @calle = 'Calle 2',
        @altura = '200',
        @id_localidad = @id_localidad,
        @id_genero = @id_genero_f,
        @correo = 'admin2@example.com',
        @celular = '1100000002';
END

SELECT @id_persona_admin2 = id_persona FROM personas WHERE num_doc = '20000002';

IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'admin2') AND @id_persona_admin2 IS NOT NULL
BEGIN
    SET @pass2 = 0x3FFA80ABC142A9AC6EBAC9BEAB81048E5DFA827FEACDD346B6B41320B7986363;
    DECLARE @now2 DATETIME = GETDATE();
    EXEC sp_insert_usuario
        @usuario = 'admin2',
        @contrasena_script = @pass2,
        @id_persona = @id_persona_admin2,
        @fecha_bloqueo = '99991231',
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = @now2,
        @id_rol = @rol_admin,
        @CambioContrasenaObligatorio = 1
END

PRINT 'Seed demo users completed.';

-- Force update passwords to ensure they are correct even if users already existed
UPDATE usuarios SET contrasena_script = 0xA6CD645C57030D00EB8F8CB4A2B21BBEDC54181871ACE4BB6E578D67337F4C05 WHERE usuario = 'admin';
UPDATE usuarios SET contrasena_script = 0x2F87B976666AF7E41D8C279FDAFB8BD8926B6D4A0684BB14F88A01A11E124C7B WHERE usuario = 'operador';
UPDATE usuarios SET contrasena_script = 0x3FFA80ABC142A9AC6EBAC9BEAB81048E5DFA827FEACDD346B6B41320B7986363 WHERE usuario = 'admin2';

-- =============================================
-- SEED DATA: Formas de Pago, Categorías, Marcas, Clientes y Productos
-- Idempotente: sólo inserta si no existe cada registro clave
-- Ejecutar tras crear toda la estructura.
-- =============================================
BEGIN TRAN;

-- Formas de Pago básicas
IF NOT EXISTS (SELECT 1 FROM FormaPago WHERE descripcion = 'Contado')
    INSERT INTO FormaPago (descripcion) VALUES ('Contado');
IF NOT EXISTS (SELECT 1 FROM FormaPago WHERE descripcion = 'Transferencia')
    INSERT INTO FormaPago (descripcion) VALUES ('Transferencia');
IF NOT EXISTS (SELECT 1 FROM FormaPago WHERE descripcion = 'Tarjeta Crédito')
    INSERT INTO FormaPago (descripcion) VALUES ('Tarjeta Crédito');

-- Categorías de Productos
IF NOT EXISTS (SELECT 1 FROM CategoriasProducto WHERE categoria = 'Alimentos')
    INSERT INTO CategoriasProducto (categoria, descripcion) VALUES ('Alimentos','Consumo diario');
IF NOT EXISTS (SELECT 1 FROM CategoriasProducto WHERE categoria = 'Bebidas')
    INSERT INTO CategoriasProducto (categoria, descripcion) VALUES ('Bebidas','Gaseosas y aguas');
IF NOT EXISTS (SELECT 1 FROM CategoriasProducto WHERE categoria = 'Limpieza')
    INSERT INTO CategoriasProducto (categoria, descripcion) VALUES ('Limpieza','Hogar y superficies');
IF NOT EXISTS (SELECT 1 FROM CategoriasProducto WHERE categoria = 'Higiene')
    INSERT INTO CategoriasProducto (categoria, descripcion) VALUES ('Higiene','Cuidado personal');

-- Marcas
IF NOT EXISTS (SELECT 1 FROM MarcasProducto WHERE marca = 'Genérica')
    INSERT INTO MarcasProducto (marca) VALUES ('Genérica');
IF NOT EXISTS (SELECT 1 FROM MarcasProducto WHERE marca = 'Acme')
    INSERT INTO MarcasProducto (marca) VALUES ('Acme');
IF NOT EXISTS (SELECT 1 FROM MarcasProducto WHERE marca = 'PremiumCo')
    INSERT INTO MarcasProducto (marca) VALUES ('PremiumCo');

-- Variables para FK
DECLARE @idFormaContado INT = (SELECT TOP 1 id_formaPago FROM FormaPago WHERE descripcion='Contado');
DECLARE @idFormaTransf INT = (SELECT TOP 1 id_formaPago FROM FormaPago WHERE descripcion='Transferencia');
DECLARE @idFormaTarjeta INT = (SELECT TOP 1 id_formaPago FROM FormaPago WHERE descripcion='Tarjeta Crédito');
DECLARE @idCatAlimentos INT = (SELECT TOP 1 id_categoria FROM CategoriasProducto WHERE categoria='Alimentos');
DECLARE @idCatBebidas INT = (SELECT TOP 1 id_categoria FROM CategoriasProducto WHERE categoria='Bebidas');
DECLARE @idCatLimpieza INT = (SELECT TOP 1 id_categoria FROM CategoriasProducto WHERE categoria='Limpieza');
DECLARE @idMarcaGenerica INT = (SELECT TOP 1 id_marca FROM MarcasProducto WHERE marca='Genérica');
DECLARE @idMarcaAcme INT = (SELECT TOP 1 id_marca FROM MarcasProducto WHERE marca='Acme');
DECLARE @idMarcaPremium INT = (SELECT TOP 1 id_marca FROM MarcasProducto WHERE marca='PremiumCo');

-- Clientes de prueba
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE codigo='CLI-0001')
    INSERT INTO Clientes (codigo,nombre,razonSocial,CUIT_DNI,id_formaPago,limiteCredito,descuento,estado)
    VALUES ('CLI-0001','Mercado Central','Mercado Central S.A.','30701234001',@idFormaContado,50000,5,'Activo');
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE codigo='CLI-0002')
    INSERT INTO Clientes (codigo,nombre,razonSocial,CUIT_DNI,id_formaPago,limiteCredito,descuento,estado)
    VALUES ('CLI-0002','Distribuidora Norte','Distribuidora Norte SRL','30711234002',@idFormaTransf,120000,7.5,'Activo');
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE codigo='CLI-0003')
    INSERT INTO Clientes (codigo,nombre,razonSocial,CUIT_DNI,id_formaPago,limiteCredito,descuento,estado)
    VALUES ('CLI-0003','Kiosco Avenida','Kiosco Avenida','20333444003',@idFormaContado,15000,0,'Activo');
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE codigo='CLI-0004')
    INSERT INTO Clientes (codigo,nombre,razonSocial,CUIT_DNI,id_formaPago,limiteCredito,descuento,estado)
    VALUES ('CLI-0004','Super Demo','Supermercado Demo S.A.','30733444004',@idFormaTarjeta,250000,3,'Activo');
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE codigo='CLI-0005')
    INSERT INTO Clientes (codigo,nombre,razonSocial,CUIT_DNI,id_formaPago,limiteCredito,descuento,estado)
    VALUES ('CLI-0005','Restó Plaza','Restó Plaza SAS','23344555005',@idFormaTransf,80000,2.5,'Activo');

-- Productos de prueba
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0001')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0001','779000100001','Arroz Largo Fino 1Kg','Bolsa de arroz largo fino',@idMarcaGenerica,450.00,620.00,'Activo','ALM-A1',1,@idCatAlimentos,'Kg',1.00,NULL,30,NULL,0,0);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0002')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0002','779000200002','Gaseosa Cola 2L','Botella PET',@idMarcaAcme,850.00,1150.00,'Activo','BEB-B2',1,@idCatBebidas,'Lt',2.00,2.00,40,180,0,1);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0003')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0003','779000300003','Detergente Líquido 500ml','Limpieza de vajilla',@idMarcaGenerica,320.00,540.00,'Activo','LIM-C1',1,@idCatLimpieza,'ml',0.50,0.50,25,730,0,0);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0004')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0004','779000400004','Shampoo Hidratante 400ml','Cuidado capilar',@idMarcaPremium,900.00,1450.00,'Activo','HIG-D4',1,@idCatAlimentos,'ml',0.40,0.40,15,730,0,0); -- usa categoría alimentos? ajustar si hay categoría Higiene separada
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0005')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0005','779000500005','Harina 000 1Kg','Harina de trigo',@idMarcaGenerica,380.00,560.00,'Activo','ALM-A2',1,@idCatAlimentos,'Kg',1.00,NULL,35,NULL,0,0);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0006')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0006','779000600006','Agua Mineral 1.5L','Agua baja en sodio',@idMarcaAcme,300.00,470.00,'Activo','BEB-B3',1,@idCatBebidas,'Lt',1.50,1.50,45,365,0,0);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0007')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0007','779000700007','Lavandina 1L','Desinfección general',@idMarcaGenerica,250.00,410.00,'Activo','LIM-C2',1,@idCatLimpieza,'Lt',1.00,1.00,20,365,0,0);
IF NOT EXISTS (SELECT 1 FROM Productos WHERE codigo='PRD-0008')
    INSERT INTO Productos (codigo,codBarras,nombre,descripcion,id_marca,precioCompra,precioVenta,estado,ubicacion,habilitado,id_categoria,unidadMedida,peso,volumen,puntoReposicion,diasVencimiento,loteObligatorio,controlVencimiento)
    VALUES ('PRD-0008','779000800008','Yerba Mate 1Kg','Yerba elaborada suave',@idMarcaPremium,1100.00,1580.00,'Activo','ALM-A3',1,@idCatAlimentos,'Kg',1.00,NULL,30,540,0,0);

COMMIT;
PRINT 'Seed clientes y productos completado.';

-- =============================================
-- SEED DATA: Stock inicial para productos demo
-- Crea filas de stock si no existen por producto
-- =============================================
DECLARE @idUsuarioAdmin INT = (SELECT TOP 1 id_usuario FROM usuarios WHERE usuario = 'admin');
IF @idUsuarioAdmin IS NULL
    SET @idUsuarioAdmin = (SELECT TOP 1 id_usuario FROM usuarios ORDER BY id_usuario);

-- Helper: upsert stock si no existe por código de producto
IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0001')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 120, 30, 80, 300, 'Disponible', 40, NULL, 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0001';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0002')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 60, 40, 80, 200, 'Disponible', 50, DATEADD(DAY, 180, GETDATE()), 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0002';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0003')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 85, 25, 60, 180, 'Disponible', 35, NULL, 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0003';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0004')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 35, 15, 40, 120, 'Disponible', 20, DATEADD(DAY, 365, GETDATE()), 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0004';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0005')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 140, 35, 90, 260, 'Disponible', 45, NULL, 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0005';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0006')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 95, 45, 85, 240, 'Disponible', 55, DATEADD(DAY, 365, GETDATE()), 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0006';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0007')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 70, 20, 50, 180, 'Disponible', 30, NULL, 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0007';

IF NOT EXISTS (SELECT 1 FROM Stock s JOIN Productos p ON s.id_producto = p.id_producto WHERE p.codigo = 'PRD-0008')
INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
SELECT p.id_producto, @idUsuarioAdmin, NULL, 110, 30, 80, 260, 'Disponible', 40, DATEADD(DAY, 540, GETDATE()), 'Habilitado', NULL FROM Productos p WHERE p.codigo='PRD-0008';

PRINT 'Seed stock inicial completado.';

-- =============================================
-- SEED DATA: Proveedores y relaciones Producto-Proveedor
-- =============================================
BEGIN TRAN;

DECLARE @idFormaContado2 INT = (SELECT TOP 1 id_formaPago FROM FormaPago WHERE descripcion='Contado');
DECLARE @idFormaTransf2 INT = (SELECT TOP 1 id_formaPago FROM FormaPago WHERE descripcion='Transferencia');

-- Proveedores demo
IF NOT EXISTS (SELECT 1 FROM Proveedores WHERE codigo='PROV-0001')
INSERT INTO Proveedores (codigo,nombre,razonSocial,CUIT,Email,Telefono,Direccion,Provincia,Ciudad,CondicionIVA,PlazoPagoDias,Observaciones,TiempoEntrega,Descuento,id_formaPago,formaPago)
VALUES ('PROV-0001','Tech Supplies SA','Tech Supplies SA','30-12345678-9','contacto@techsupplies.com','011-5555-0001','Av. Siempre Viva 123','CABA','Buenos Aires','Responsable Inscripto',30,'Proveedor general',5,0,@idFormaTransf2,'Transferencia');

IF NOT EXISTS (SELECT 1 FROM Proveedores WHERE codigo='PROV-0002')
INSERT INTO Proveedores (codigo,nombre,razonSocial,CUIT,Email,Telefono,Direccion,Provincia,Ciudad,CondicionIVA,PlazoPagoDias,Observaciones,TiempoEntrega,Descuento,id_formaPago,formaPago)
VALUES ('PROV-0002','Distribuidora Norte','Distribuidora Norte SRL','30-87654321-0','ventas@norte.com','381-444-2211','Belgrano 450','Tucumán','San Miguel','Monotributo',45,'Distribución regional',7,2.5,@idFormaTransf2,'Transferencia');

IF NOT EXISTS (SELECT 1 FROM Proveedores WHERE codigo='PROV-0003')
INSERT INTO Proveedores (codigo,nombre,razonSocial,CUIT,Email,Telefono,Direccion,Provincia,Ciudad,CondicionIVA,PlazoPagoDias,Observaciones,TiempoEntrega,Descuento,id_formaPago,formaPago)
VALUES ('PROV-0003','Bebidas Sur','Bebidas del Sur SA','30-33445566-7','contacto@bebidassur.com','0291-400-1122','Ruta 3 Km 5','Buenos Aires','Bahía Blanca','Responsable Inscripto',20,'Bebidas y aguas',4,1.0,@idFormaContado2,'Contado');

-- Relacionar productos con proveedores (si no existe la relación)
DECLARE @prov1 INT = (SELECT TOP 1 id_proveedor FROM Proveedores WHERE codigo='PROV-0001');
DECLARE @prov2 INT = (SELECT TOP 1 id_proveedor FROM Proveedores WHERE codigo='PROV-0002');
DECLARE @prov3 INT = (SELECT TOP 1 id_proveedor FROM Proveedores WHERE codigo='PROV-0003');

-- Helper to get product id by code
DECLARE @p1 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0001');
DECLARE @p2 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0002');
DECLARE @p3 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0003');
DECLARE @p4 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0004');
DECLARE @p5 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0005');
DECLARE @p6 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0006');
DECLARE @p7 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0007');
DECLARE @p8 INT = (SELECT TOP 1 id_producto FROM Productos WHERE codigo='PRD-0008');

-- Inserts idempotentes en ProductoProveedor
IF @prov1 IS NOT NULL AND @p1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov1 AND id_producto=@p1)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov1,@p1,450.00,5,0);
IF @prov1 IS NOT NULL AND @p5 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov1 AND id_producto=@p5)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov1,@p5,380.00,5,0);
IF @prov1 IS NOT NULL AND @p4 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov1 AND id_producto=@p4)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov1,@p4,900.00,7,1.0);
IF @prov1 IS NOT NULL AND @p8 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov1 AND id_producto=@p8)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov1,@p8,1100.00,6,0);

IF @prov2 IS NOT NULL AND @p3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov2 AND id_producto=@p3)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov2,@p3,320.00,4,0);
IF @prov2 IS NOT NULL AND @p7 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov2 AND id_producto=@p7)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov2,@p7,250.00,4,0);

IF @prov3 IS NOT NULL AND @p2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov3 AND id_producto=@p2)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov3,@p2,850.00,3,0);
IF @prov3 IS NOT NULL AND @p6 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ProductoProveedor WHERE id_proveedor=@prov3 AND id_producto=@p6)
    INSERT INTO ProductoProveedor (id_proveedor,id_producto,precioCompra,tiempoEntrega,descuento) VALUES (@prov3,@p6,300.00,2,0);

COMMIT;
PRINT 'Seed proveedores y relaciones producto-proveedor completado.';
