using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.App.Services;

namespace AgileStockPro.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddFluentUIComponents();

        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<ISupplierService, SupplierService>();
        builder.Services.AddSingleton<IDashboardService, DashboardService>();
        builder.Services.AddSingleton<IAlmacenService, AlmacenService>();
        builder.Services.AddSingleton<IClienteService, ClienteService>();
        builder.Services.AddSingleton<ICompraService, CompraService>();
        builder.Services.AddSingleton<IOrdenCompraService, OrdenCompraService>();
        builder.Services.AddSingleton<FacturacionService>();
        builder.Services.AddSingleton<MovimientoService>();
        builder.Services.AddSingleton<ScrapService>();
        builder.Services.AddSingleton<SettingsService>();

        return builder.Build();
    }
}
