-- Update sp_get_users to support @RoleId filter
SET ANSI_NULLS ON
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