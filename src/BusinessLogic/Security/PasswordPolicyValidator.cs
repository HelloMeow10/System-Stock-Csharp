using DataAccess.Entities;
using System;
using System.Linq;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Security
{
    public class PasswordPolicyValidator : IPasswordPolicyValidator
    {
        public void Validate(string password, string username, Persona persona, PoliticaSeguridad politica)
        {
            if (politica == null) return;

            if (password.Length < politica.MinCaracteres)
                throw new ValidationException($"La contraseña debe tener al menos {politica.MinCaracteres} caracteres.");

            if (politica.MayusYMinus && (!password.Any(char.IsUpper) || !password.Any(char.IsLower)))
                throw new ValidationException("La contraseña debe contener mayúsculas y minúsculas.");

            if (politica.LetrasYNumeros && (!password.Any(char.IsLetter) || !password.Any(char.IsDigit)))
                throw new ValidationException("La contraseña debe contener letras y números.");

            if (politica.CaracterEspecial && !password.Any(c => !char.IsLetterOrDigit(c)))
                throw new ValidationException("La contraseña debe contener caracteres especiales.");

            if (politica.SinDatosPersonales)
            {
                if (password.Contains(username, StringComparison.OrdinalIgnoreCase) ||
                    password.Contains(persona.Nombre, StringComparison.OrdinalIgnoreCase) ||
                    password.Contains(persona.Apellido, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ValidationException("La contraseña no debe contener datos personales (nombre de usuario, nombre o apellido).");
                }

                if (persona.FechaNacimiento.HasValue)
                {
                    string[] dateFormats = { "ddMMyyyy", "yyyyMMdd", "ddMM", "MMdd" };
                    foreach (var format in dateFormats)
                    {
                        if (password.Contains(persona.FechaNacimiento.Value.ToString(format)))
                        {
                            throw new ValidationException("La contraseña no debe contener su fecha de nacimiento.");
                        }
                    }
                }
            }
        }
    }
}
