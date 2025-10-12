using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (classic Blazor Server)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Per Fluent docs, register HttpClient before AddFluentUIComponents
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

// Register app services (start with Products; add more as you port them)
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IDashboardService, DashboardService>();
builder.Services.AddSingleton<IAppDataService, AppDataService>();
builder.Services.AddScoped<IUserStore, LocalUserStore>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
