using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
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
            ErrorResponse errorResponse;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    errorResponse = new ErrorResponse
                    {
                        Message = "Se produjeron uno o más errores de validación.",
                        Errors = validationException.Errors
                    };
                    break;
                case BusinessLogicException businessLogicException when businessLogicException.Message.Contains("not found"):
                    code = HttpStatusCode.NotFound;
                    errorResponse = new ErrorResponse { Message = businessLogicException.Message };
                    break;
                case BusinessLogicException businessLogicException:
                    code = HttpStatusCode.Conflict; // Using 409 Conflict for business rule violations
                    errorResponse = new ErrorResponse { Message = businessLogicException.Message };
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    errorResponse = new ErrorResponse { Message = "Ocurrió un error interno inesperado en el servidor." };
                    if (_env.IsDevelopment())
                    {
                        errorResponse.Errors = new[] { exception.ToString() };
                    }
                    break;
            }

            _logger.LogError(exception, "Una excepción fue manejada por el middleware: {Message}", exception.Message);

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
