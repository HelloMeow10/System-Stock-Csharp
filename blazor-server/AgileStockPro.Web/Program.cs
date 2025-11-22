using Microsoft.FluentUI.AspNetCore.Components;
using AgileStockPro.Web.Services;
using BusinessLogic;
using DataAccess;
using Microsoft.AspNetCore.Components.Authorization;
using AgileStockPro.Web.Auth;

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
