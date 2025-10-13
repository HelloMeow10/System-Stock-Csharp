using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (classic Blazor Server)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

// Per Fluent docs, register HttpClient before AddFluentUIComponents
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

// Register app services (start with Products; add more as you port them)
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IDashboardService, DashboardService>();
builder.Services.AddSingleton<IAppDataService, AppDataService>();

// API integration
builder.Services.AddSingleton(new AgileStockPro.Web.Services.Api.ApiOptions { BaseUrl = "http://localhost:5000/" });
builder.Services.AddScoped<AgileStockPro.Web.Services.Api.ITokenProvider, AgileStockPro.Web.Services.Api.TokenProvider>();
builder.Services.AddHttpClient<AgileStockPro.Web.Services.Api.BackendApiClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer()
    });
builder.Services.AddScoped<IUserStore, AgileStockPro.Web.Services.Api.ApiUserStore>();
builder.Services.AddScoped<IAuthService, AgileStockPro.Web.Services.Api.ApiAuthService>();

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
