using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic.Mappers
{
    public static class PersonaMapper
    {
        public static PersonaDto? MapToPersonaDto(Persona? p)
        {
            if (p == null) return null;
            return new PersonaDto
            {
                IdPersona = p.IdPersona,
                Legajo = p.Legajo,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                NombreCompleto = p.NombreCompleto,
                IdTipoDoc = p.IdTipoDoc,
                TipoDocNombre = p.TipoDoc?.Nombre,
                NumDoc = p.NumDoc,
                FechaNacimiento = p.FechaNacimiento,
                Cuil = p.Cuil,
                Calle = p.Calle,
                Altura = p.Altura,
                IdLocalidad = p.IdLocalidad,
                LocalidadNombre = p.Localidad?.Nombre,
                IdPartido = p.Localidad?.IdPartido ?? 0,
                PartidoNombre = p.Localidad?.Partido?.Nombre,
                IdProvincia = p.Localidad?.Partido?.IdProvincia ?? 0,
                ProvinciaNombre = p.Localidad?.Partido?.Provincia?.Nombre,
                IdGenero = p.IdGenero,
                GeneroNombre = p.Genero?.Nombre,
                Correo = p.Correo,
                Celular = p.Celular,
                FechaIngreso = p.FechaIngreso
            };
        }
    }
}
