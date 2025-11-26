using BusinessLogic.Configuration;
using BusinessLogic.Factories;
using BusinessLogic.Security;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Register configuration
            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));

            // Register Factories
            services.AddScoped<IPersonaFactory, PersonaFactory>();
            services.AddScoped<IUsuarioFactory, UsuarioFactory>();

            // Register Security Services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordPolicyValidator, PasswordPolicyValidator>();

            // Register Main Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPersonaService, PersonaService>();
            // Stock/Product Catalog
            services.AddScoped<IProductCatalogService, ProductCatalogService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();
            services.AddScoped<ISecurityPolicyService, SecurityPolicyService>();
            services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();
            services.AddScoped<IUserService, UserManagementService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IProductSupplierService, ProductSupplierService>();
            services.AddScoped<IPurchasingService, PurchasingService>();
            services.AddScoped<IInvoicingService, InvoicingService>();

            return services;
        }
    }
}
