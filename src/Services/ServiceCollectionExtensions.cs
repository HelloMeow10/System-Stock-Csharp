using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Contracts;
using Microsoft.OpenApi.Models;
using Services.Hateoas;
using Session;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Services.Swagger;

namespace Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor, Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor>();
            services.AddScoped<ITokenService, TokenService>();

            // Register HATEOAS services
            services.AddScoped<HateoasActionFilter>();
            services.AddScoped<ILinkFactory<UserDto>, UserLinksFactory>();
            services.AddScoped<ILinkFactory<UserDtoV2>, UserLinksFactoryV2>();
            services.AddScoped<ILinkFactory<PersonaDto>, PersonaLinksFactory>();
            services.AddScoped<ILinkFactory<PoliticaSeguridadDto>, PoliticaSeguridadLinksFactory>();

            // Register the generic factory for paged responses by its concrete type
            services.AddScoped(typeof(PagedResponseLinksFactory<>));

            return services;
        }

        public static IServiceCollection AddApiVersioningServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                })
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthSchemeOptions.Scheme, null);
            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                // The XML comments and security definitions are now handled by ConfigureSwaggerOptions
            });

            return services;
        }
    }
}
