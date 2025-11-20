using Asp.Versioning.ApiExplorer;
using BusinessLogic;
using DataAccess;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Services;
using Services.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---

// SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddApiVersioningServices();
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "database",
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] { "ready" });

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerDocumentation();


// --- Application Pipeline Configuration ---

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(options => { });


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false
});


// Map SignalR hub for notifications
app.MapHub<Services.Hubs.NotificationHub>("/hubs/notifications");


app.Run();

// Make the implicit Program class public so it can be referenced by the test project
public partial class Program { }
