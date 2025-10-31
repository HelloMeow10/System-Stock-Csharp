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
    RAISERROR('Catálogo mínimo faltante (tipo_doc/generos/localidades). Ejecuta el script de creación base primero.', 16, 1);
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
    SET @pass = HASHBYTES('SHA2_256', 'Operador123operador');
    EXEC sp_insert_usuario
        @usuario = 'operador',
        @contrasena_script = @pass,
        @id_persona = @id_persona_oper,
        @fecha_bloqueo = '9999-12-31',
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = GETDATE(),
        @id_rol = @rol_user,
        @CambioContrasenaObligatorio = 1
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
    SET @pass2 = HASHBYTES('SHA2_256', 'Admin123admin');
    EXEC sp_insert_usuario
        @usuario = 'admin2',
        @contrasena_script = @pass2,
        @id_persona = @id_persona_admin2,
        @fecha_bloqueo = '9999-12-31',
        @nombre_usuario_bloqueo = NULL,
        @fecha_ultimo_cambio = GETDATE(),
        @id_rol = @rol_admin,
        @CambioContrasenaObligatorio = 1
END

PRINT 'Seed demo users completed.';

PRINT 'Seed demo users completed.';