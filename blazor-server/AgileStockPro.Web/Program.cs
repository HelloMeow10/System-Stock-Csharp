using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.Web.Services;
using BusinessLogic;
using DataAccess;
using Microsoft.AspNetCore.Components.Authorization;
using AgileStockPro.Web.Auth;
using AgileStockPro.Web.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(o => o.DetailedErrors = true);
builder.Services.AddHttpContextAccessor();

// Fluent UI
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

// Backend Services (N-Layer)
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddBusinessLogic(builder.Configuration);

// Authentication
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies");

// API options & token provider & backend client
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
// Development: use HTTP base URL (backend HTTPS redirection disabled for now)
builder.Services.AddSingleton(new ApiOptions { BaseUrl = "http://localhost:5000/" });
builder.Services.AddScoped<BackendApiClient>();

// App services (Auth, User store, Toast, etc.)
// Switch to API-backed auth service (real backend + JWT) and API user store
builder.Services.AddScoped<IAuthService, ApiAuthService>();
builder.Services.AddScoped<AgileStockPro.Web.Services.IUserStore, AgileStockPro.Web.Services.Api.ApiUserStore>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ISecurityQuestionsAdminService, ApiSecurityQuestionsService>();
// Data services
builder.Services.AddScoped<AppDataService>(); // fallback sample data
builder.Services.AddScoped<IAppDataService, ApiAppDataService>(); // API-backed with fallback
builder.Services.AddScoped<AgileStockPro.Web.Services.Api.IStockApiService, AgileStockPro.Web.Services.Api.StockApiService>();
// Products
builder.Services.AddScoped<AgileStockPro.Web.Services.IProductService, AgileStockPro.Web.Services.Api.ApiProductService>();
builder.Services.AddScoped<AgileStockPro.Web.Services.Api.ApiProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
