using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception has occurred: {Message}", exception.Message);

            var problemDetails = CreateProblemDetails(httpContext, exception);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case AuthenticationException authException:
                    problemDetails.Title = "Authentication Error";
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                    problemDetails.Detail = authException.Message;
                    break;

                case ValidationException validationException:
                    problemDetails.Title = "Validation Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    problemDetails.Detail = "One or more validation errors occurred.";
                    problemDetails.Extensions["errors"] = validationException.Errors;
                    break;

                case BusinessLogicException businessLogicException when businessLogicException.Message.Contains("not found", StringComparison.OrdinalIgnoreCase):
                    problemDetails.Title = "Resource Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                    problemDetails.Detail = businessLogicException.Message;
                    break;

                case BusinessLogicException businessLogicException:
                    problemDetails.Title = "Business Rule Violation";
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
                    problemDetails.Detail = businessLogicException.Message;
                    break;

                default:
                    problemDetails.Title = "An unexpected server error occurred.";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    problemDetails.Detail = _env.IsDevelopment() ? exception.ToString() : "An internal server error has occurred.";
                    break;
            }

            return problemDetails;
        }
    }
}
