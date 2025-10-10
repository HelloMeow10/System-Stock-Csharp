using AutoMapper;
using Contracts;
using DataAccess.Entities;
using System;

namespace BusinessLogic.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping from (Usuario, PersonaDto) tuple to UserDto (v1)
            CreateMap<(Usuario, PersonaDto), UserDto>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Item1.IdUsuario))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Item1.UsuarioNombre))
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Item1.Rol.Nombre))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Item1.IdRol))
                .ForMember(dest => dest.IdPersona, opt => opt.MapFrom(src => src.Item1.IdPersona))
                .ForMember(dest => dest.CambioContrasenaObligatorio, opt => opt.MapFrom(src => src.Item1.CambioContrasenaObligatorio))
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src => src.Item1.FechaExpiracion))
                .ForMember(dest => dest.Habilitado, opt => opt.MapFrom(src => src.Item1.FechaBloqueo == null || src.Item1.FechaBloqueo > DateTime.Now))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.Nombre : null))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.Apellido : null))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.Correo : null));

            // Mapping from (Usuario, PersonaDto) tuple to UserDtoV2
            CreateMap<(Usuario, PersonaDto), UserDtoV2>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Item1.IdUsuario))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Item1.UsuarioNombre))
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Item1.Rol.Nombre))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Item1.IdRol))
                .ForMember(dest => dest.IdPersona, opt => opt.MapFrom(src => src.Item1.IdPersona))
                .ForMember(dest => dest.CambioContrasenaObligatorio, opt => opt.MapFrom(src => src.Item1.CambioContrasenaObligatorio))
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src => src.Item1.FechaExpiracion))
                .ForMember(dest => dest.Habilitado, opt => opt.MapFrom(src => src.Item1.FechaBloqueo == null || src.Item1.FechaBloqueo > DateTime.Now))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.NombreCompleto : null))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Item2 != null ? src.Item2.Correo : null));

            // Mapping from DTO to Request objects for PATCH operations
            CreateMap<UserDto, UpdateUserRequest>();
            CreateMap<UserDtoV2, UpdateUserRequestV2>();

            // Mappings for UpdateUserRequest (v1) to entities
            CreateMap<UpdateUserRequest, Persona>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateUserRequest, Usuario>()
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src => src.FechaExpiracion))
                .ForMember(dest => dest.CambioContrasenaObligatorio, opt => opt.MapFrom(src => src.CambioContrasenaObligatorio))
                .AfterMap((src, dest) => {
                    if (src.Habilitado)
                    {
                        dest.Habilitar();
                    }
                    else
                    {
                        dest.Deshabilitar("Admin"); // Or use a proper actor
                    }
                })
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Mappings for UpdateUserRequestV2 to entities
            CreateMap<UpdateUserRequestV2, Persona>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[0]))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries).Length > 1
                                                                    ? src.FullName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[1]
                                                                    : ""))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateUserRequestV2, Usuario>()
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src => src.FechaExpiracion))
                .ForMember(dest => dest.CambioContrasenaObligatorio, opt => opt.MapFrom(src => src.CambioContrasenaObligatorio))
                .AfterMap((src, dest) => {
                    if (src.Habilitado)
                    {
                        dest.Habilitar();
                    }
                    else
                    {
                        dest.Deshabilitar("Admin"); // Or use a proper actor
                    }
                })
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}