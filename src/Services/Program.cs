using Microsoft.IdentityModel.Tokens;
using System.Text;
using Services.Middleware;
using Session;
using BusinessLogic; // Import the namespace for AddInfrastructure
using Services.Hateoas;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Add infrastructure services from the BusinessLogic layer
builder.Services.AddInfrastructure(builder.Configuration);

// Add services specific to the API layer
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILinkService, LinkService>();


builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer(); // Required for API Explorer
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1",
        Description = "An API for managing users, persons, and security policies."
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Enable Swagger UI only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
