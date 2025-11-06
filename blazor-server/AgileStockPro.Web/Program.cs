using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.Web.Services;
using AgileStockPro.Web.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (classic Blazor Server)
builder.Services.AddRazorPages();
var blazorBuilder = builder.Services.AddServerSideBlazor();
if (builder.Environment.IsDevelopment())
{
    blazorBuilder.AddCircuitOptions(o => o.DetailedErrors = true);
}
builder.Services.AddHttpContextAccessor();

// Per Fluent docs, register HttpClient before AddFluentUIComponents
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

// Register app services (start with Products; add more as you port them)
var useApiProducts = builder.Configuration.GetValue<bool>("Features:Products:UseApi", true);
if (useApiProducts)
{
    builder.Services.AddScoped<IProductService, AgileStockPro.Web.Services.Api.ApiProductService>();
}
else
{
    builder.Services.AddSingleton<IProductService, ProductService>();
}
builder.Services.AddSingleton<IDashboardService, DashboardService>();

// App data (movements/scrap from API with fallback to local mock)
var useApiAppData = builder.Configuration.GetValue<bool>("Features:AppData:UseApi", true);
if (useApiAppData)
{
    builder.Services.AddSingleton<AppDataService>(); // fallback instance
    builder.Services.AddScoped<IAppDataService, AgileStockPro.Web.Services.Api.ApiAppDataService>();
}
else
{
    builder.Services.AddSingleton<IAppDataService, AppDataService>();
}

// API integration
builder.Services.AddSingleton(new ApiOptions { BaseUrl = "http://localhost:5000/" });
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddHttpClient<BackendApiClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer()
    });
builder.Services.AddScoped<IUserStore, ApiUserStore>();
builder.Services.AddScoped<IAuthService, ApiAuthService>();
builder.Services.AddScoped<IStockApiService, StockApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
