using BusinessLogic.Configuration;
using BusinessLogic.Security;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register configuration
            services.AddSingleton(configuration);

            // Configure SmtpSettings using the options pattern
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            // Register logging
            services.AddSingleton<ILoggerFactory>(serviceProvider =>
            {
                return LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddConsole();
                });
            });

            services.AddLogging();

            // Register DataAccess components
            services.AddSingleton<DatabaseConnectionFactory>();
            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddScoped<IPersonaRepository, SqlPersonaRepository>();
            services.AddScoped<ISecurityRepository, SqlSecurityRepository>();
            services.AddScoped<IReferenceDataRepository, SqlReferenceDataRepository>();

            // Register BusinessLogic factories
            services.AddTransient<IPersonaFactory, PersonaFactory>();
            services.AddTransient<IUsuarioFactory, UsuarioFactory>();

            // Register BusinessLogic services
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPasswordPolicyValidator, PasswordPolicyValidator>();

            // Register new granular services
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IPersonaService, PersonaService>();

            services.AddTransient<ISecurityQuestionService, SecurityQuestionService>();
            services.AddTransient<ISecurityPolicyService, SecurityPolicyService>();

            services.AddTransient<IUserService, UserManagementService>();
            services.AddTransient<IReferenceDataService, ReferenceDataService>();

            return services;
        }
    }
}
