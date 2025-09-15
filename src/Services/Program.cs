using BusinessLogic;
using Services;
using Services.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- Service Registration ---

// Add services to the container.
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the implicit Program class public so it can be referenced by the test project
public partial class Program { }
