using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddSingleton<DatabaseConnectionFactory>();
            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddScoped<IPersonaRepository, SqlPersonaRepository>();
            services.AddScoped<ISecurityRepository, SqlSecurityRepository>();
            services.AddScoped<IReferenceDataRepository, SqlReferenceDataRepository>();

            return services;
        }
    }
}
