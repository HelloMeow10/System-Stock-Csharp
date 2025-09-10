using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
            ProblemDetails problemDetails;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)code,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "Validation error",
                        Detail = string.Join(", ", validationException.Errors)
                    };
                    break;
                case BusinessLogicException businessLogicException when businessLogicException.Message.Contains("not found"):
                    code = HttpStatusCode.NotFound;
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)code,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Title = "Resource not found",
                        Detail = businessLogicException.Message
                    };
                    break;
                case BusinessLogicException businessLogicException:
                    code = HttpStatusCode.Conflict; // Using 409 Conflict for business rule violations
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)code,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                        Title = "Business logic error",
                        Detail = businessLogicException.Message
                    };
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)code,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                        Title = "An unexpected internal server error has occurred."
                    };
                    if (_env.IsDevelopment())
                    {
                        problemDetails.Detail = exception.ToString();
                    }
                    break;
            }

            _logger.LogError(exception, "An exception was handled by the middleware: {Message}", exception.Message);

            var result = JsonSerializer.Serialize(problemDetails);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
