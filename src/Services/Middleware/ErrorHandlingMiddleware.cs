using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Contracts; // Add this using directive

namespace Services.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            ApiResponse<object> response;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail(validationException.Errors.ToList());
                    break;
                case BusinessLogicException businessLogicException when businessLogicException.Message.Contains("not found"):
                    code = HttpStatusCode.NotFound;
                    response = ApiResponse<object>.Fail(businessLogicException.Message);
                    break;
                case BusinessLogicException businessLogicException:
                    code = HttpStatusCode.Conflict; // Using 409 Conflict for business rule violations
                    response = ApiResponse<object>.Fail(businessLogicException.Message);
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    var errorList = new List<string> { "Ocurrió un error interno inesperado en el servidor." };
                    if (_env.IsDevelopment())
                    {
                        errorList.Add(exception.ToString());
                    }
                    response = ApiResponse<object>.Fail(errorList);
                    break;
            }

            _logger.LogError(exception, "Una excepción fue manejada por el middleware: {Message}", exception.Message);

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
