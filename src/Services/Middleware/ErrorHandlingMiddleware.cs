using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace Services.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                    // In a real app, you might serialize validationException.Errors into the message or a separate field.
                    response = ApiResponse<object>.CreateFailure("ValidationFailure", string.Join(", ", validationException.Errors.Select(e => e.Value)));
                    break;
                case BusinessLogicException businessLogicException:
                    code = HttpStatusCode.Conflict; // Using 409 Conflict for business rule violations
                    response = ApiResponse<object>.CreateFailure("BusinessLogicError", businessLogicException.Message);
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    response = ApiResponse<object>.CreateFailure("InternalServerError", "An unexpected internal server error has occurred.");
                    break;
            }

            _logger.LogError(exception, "An exception was handled by the middleware: {Message}", exception.Message);

            var result = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
