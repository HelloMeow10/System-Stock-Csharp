using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;

namespace DataAccess.Entities
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; private set; }

        [Required]
        [Column("usuario")]
        [StringLength(30)]
        public string UsuarioNombre { get; private set; } = null!;

        [Required]
        [Column("contrasena_script")]
        [MaxLength(512)]
        public byte[] ContrasenaScript { get; private set; } = null!;

        [Column("id_persona")]
        public int IdPersona { get; private set; }

        [Column("fecha_bloqueo")]
        public DateTime FechaBloqueo { get; private set; }

        [Column("nombre_usuario_bloqueo")]
        [StringLength(30)]
        public string? NombreUsuarioBloqueo { get; private set; }

        [Column("fecha_ultimo_cambio")]
        public DateTime FechaUltimoCambio { get; private set; }

        [Column("id_rol")]
        public int IdRol { get; private set; }

        [Column("id_politica")]
        public int? IdPolitica { get; private set; }

        [Column("CambioContrasenaObligatorio")]
        public bool CambioContrasenaObligatorio { get; private set; }

        [Column("Codigo2FA")]
        [StringLength(10)]
        public string? Codigo2FA { get; private set; }

        [Column("Codigo2FAExpiracion")]
        public DateTime? Codigo2FAExpiracion { get; private set; }

        [Column("FechaExpiracion")]
        public DateTime? FechaExpiracion { get; private set; }

        [ForeignKey("IdPersona")]
        public virtual Persona Persona { get; private set; } = null!;

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; private set; } = null!;

        [ForeignKey("IdPolitica")]
        public virtual PoliticaSeguridad? PoliticaSeguridad { get; private set; }

        private Usuario() { } // EF Core constructor

        public Usuario(string usuarioNombre, byte[] contrasenaScript, int idPersona, int idRol, int? idPolitica)
        {
            UsuarioNombre = !string.IsNullOrWhiteSpace(usuarioNombre) ? usuarioNombre : throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(usuarioNombre));
            ContrasenaScript = contrasenaScript ?? throw new ArgumentNullException(nameof(contrasenaScript));
            IdPersona = idPersona;
            IdRol = idRol;
            IdPolitica = idPolitica;
            FechaUltimoCambio = DateTime.Now;
            CambioContrasenaObligatorio = true;
            Habilitar();
        }

        public void Deshabilitar(string nombreUsuarioBloqueo)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuarioBloqueo))
            {
                throw new ArgumentException("El nombre del usuario que bloquea no puede ser nulo o vacío.", nameof(nombreUsuarioBloqueo));
            }
            FechaBloqueo = DateTime.Now;
            NombreUsuarioBloqueo = nombreUsuarioBloqueo;
        }

        public void Habilitar()
        {
            FechaBloqueo = new DateTime(9999, 12, 31);
            NombreUsuarioBloqueo = null;
        }

        public void ChangeRole(int newRoleId)
        {
            IdRol = newRoleId;
        }

        public void ForcePasswordChange(bool required)
        {
            CambioContrasenaObligatorio = required;
        }

        public void SetExpiration(DateTime? expirationDate)
        {
            FechaExpiracion = expirationDate;
        }

        public void ChangePassword(byte[] newPasswordHash)
        {
            ContrasenaScript = newPasswordHash;
            FechaUltimoCambio = DateTime.Now;
            CambioContrasenaObligatorio = false;
        }

        public void SetTwoFactorCode(string? code, DateTime? expiration)
        {
            Codigo2FA = code;
            Codigo2FAExpiracion = expiration;
        }

        public Usuario(int idUsuario, string usuarioNombre, byte[] contrasenaScript, int idPersona, DateTime fechaBloqueo, string? nombreUsuarioBloqueo, DateTime fechaUltimoCambio, int idRol, int? idPolitica, bool cambioContrasenaObligatorio, string? codigo2FA, DateTime? codigo2FAExpiracion, DateTime? fechaExpiracion, Rol rol)
        {
            IdUsuario = idUsuario;
            UsuarioNombre = usuarioNombre;
            ContrasenaScript = contrasenaScript;
            IdPersona = idPersona;
            FechaBloqueo = fechaBloqueo;
            NombreUsuarioBloqueo = nombreUsuarioBloqueo;
            FechaUltimoCambio = fechaUltimoCambio;
            IdRol = idRol;
            IdPolitica = idPolitica;
            CambioContrasenaObligatorio = cambioContrasenaObligatorio;
            Codigo2FA = codigo2FA;
            Codigo2FAExpiracion = codigo2FAExpiracion;
            FechaExpiracion = fechaExpiracion;
            Rol = rol;
        }

        public static Usuario FromDataReader(SqlDataReader reader)
        {
            var rol = new Rol
            {
                IdRol = (int)reader["rol_id_rol"],
                Nombre = reader["rol"] as string ?? string.Empty
            };

            return new Usuario(
                (int)reader["id_usuario"],
                reader["usuario"] as string ?? string.Empty,
                (byte[])reader["contrasena_script"],
                (int)reader["id_persona"],
                (DateTime)reader["fecha_bloqueo"],
                reader["nombre_usuario_bloqueo"] as string,
                (DateTime)reader["fecha_ultimo_cambio"],
                (int)reader["id_rol"],
                reader["id_politica"] as int?,
                (bool)reader["CambioContrasenaObligatorio"],
                reader["Codigo2FA"] as string,
                reader["Codigo2FAExpiracion"] as DateTime?,
                reader["FechaExpiracion"] as DateTime?,
                rol
            );
        }
    }
}