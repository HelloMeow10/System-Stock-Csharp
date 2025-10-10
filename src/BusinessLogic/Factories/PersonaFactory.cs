using Contracts;
using DataAccess.Entities;
using DataAccess.Repositories;
using BusinessLogic.Exceptions;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Factories
{
    public class PersonaFactory : IPersonaFactory
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public PersonaFactory(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<Persona> CreateAsync(PersonaRequest request)
        {
            ValidatePersonaRequest(request);

            if (!int.TryParse(request.Legajo, out int legajo))
            {
                throw new ValidationException("El legajo debe ser un número válido.");
            }
            if (!int.TryParse(request.Localidad, out int localidadId))
            {
                throw new ValidationException("El ID de localidad no es válido.");
            }

            var idTipoDoc = (await _referenceDataRepository.GetTipoDocByNombreAsync(request.TipoDoc))?.IdTipoDoc
                ?? throw new ValidationException("Tipo de documento no encontrado");
            var idGenero = (await _referenceDataRepository.GetGeneroByNombreAsync(request.Genero))?.IdGenero
                ?? throw new ValidationException("Género no encontrado");

            return new Persona(
                legajo,
                request.Nombre,
                request.Apellido,
                idTipoDoc,
                request.NumDoc,
                request.FechaNacimiento,
                request.Cuil,
                request.Calle,
                request.Altura,
                localidadId,
                idGenero,
                request.Correo,
                request.Celular,
                request.FechaIngreso
            );
        }

        private void ValidatePersonaRequest(PersonaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(request.Apellido))
                throw new ValidationException("El apellido no puede estar vacío.");
            if (!long.TryParse(request.NumDoc, out _))
                throw new ValidationException("El número de documento debe ser numérico.");
            if (!string.IsNullOrWhiteSpace(request.Cuil) && !long.TryParse(request.Cuil, out _))
                throw new ValidationException("El CUIL debe ser numérico.");
            if (string.IsNullOrWhiteSpace(request.Calle))
                throw new ValidationException("La calle no puede estar vacía.");
            if (!int.TryParse(request.Altura, out _))
                throw new ValidationException("La altura de la dirección debe ser un número.");
            if (string.IsNullOrWhiteSpace(request.Correo) || !IsValidEmail(request.Correo))
                throw new ValidationException("El formato del correo electrónico no es válido.");
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}