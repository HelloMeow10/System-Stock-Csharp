using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DatabaseConnectionFactory>();
            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddScoped<IPersonaRepository, SqlPersonaRepository>();
            services.AddScoped<ISecurityRepository, SqlSecurityRepository>();
            services.AddScoped<IReferenceDataRepository, SqlReferenceDataRepository>();
            services.AddScoped<ISupplierRepository, SqlSupplierRepository>();
            services.AddScoped<IClientRepository, SqlClientRepository>();
            services.AddScoped<IPurchaseRepository, SqlPurchaseRepository>();
            services.AddScoped<ISalesRepository, SqlSalesRepository>();
            // Feature flags
            // Allow independent toggles: Catalog vs Stock
            // Backward compatible: if Catalog flag missing, fall back to Stock flag
            var useSqlStock = configuration.GetValue<bool>("Features:Stock:UseSqlRepository");
            var useSqlCatalog = configuration.GetValue<bool?>("Features:Catalog:UseSqlRepository") ?? useSqlStock;

            // Product Catalog
            if (useSqlCatalog)
                services.AddScoped<IProductCatalogRepository, SqlProductCatalogRepository>();
            else
                services.AddScoped<IProductCatalogRepository, StubProductCatalogRepository>();

            // Stock operations (toggleable for dev/demo without DB)
            if (useSqlStock)
                services.AddScoped<IStockRepository, SqlStockRepository>();
            else
                services.AddSingleton<IStockRepository, StubStockRepository>();

            return services;
        }
    }
}
