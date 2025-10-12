namespace AgileStockPro.Web.Services.Api;

public class PagedResponse<T>
{
    public IEnumerable<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class UserDto
{
    public int IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Correo { get; set; }
    public string? Rol { get; set; }
    public int IdRol { get; set; }
    public int IdPersona { get; set; }
    public bool CambioContrasenaObligatorio { get; set; }
}

public class PoliticaSeguridadDto
{
    public int IdPolitica { get; set; }
    public bool MayusYMinus { get; set; }
    public bool LetrasYNumeros { get; set; }
    public bool CaracterEspecial { get; set; }
    public bool Autenticacion2FA { get; set; }
    public bool NoRepetirAnteriores { get; set; }
    public bool SinDatosPersonales { get; set; }
    public int MinCaracteres { get; set; }
    public int CantPreguntas { get; set; }
}

public class PersonaDto
{
    public int IdPersona { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}

public class PreguntaSeguridadDto
{
    public int IdPregunta { get; set; }
    public string Pregunta { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Requires2fa { get; set; }
    public string? Username { get; set; }
    public string? Rol { get; set; }
    public string? Token { get; set; }
}

public class LoginRequest { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }
public class Validate2faRequest { public string Username { get; set; } = string.Empty; public string Code { get; set; } = string.Empty; }
public class ChangePasswordRequest { public string Username { get; set; } = string.Empty; public string OldPassword { get; set; } = string.Empty; public string NewPassword { get; set; } = string.Empty; }
public class RecoverPasswordRequest { public string Username { get; set; } = string.Empty; public Dictionary<int, string> Answers { get; set; } = new(); }
