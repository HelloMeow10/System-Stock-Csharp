using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Services.Swagger
{
    public class ErrorResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var problemDetailsSchema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

            // Add 400 Bad Request response
            operation.Responses.TryAdd("400", new OpenApiResponse
            {
                Description = "Bad Request. The server could not understand the request due to invalid syntax.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 404 Not Found response
            operation.Responses.TryAdd("404", new OpenApiResponse
            {
                Description = "Not Found. The requested resource could not be found.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 409 Conflict response
            operation.Responses.TryAdd("409", new OpenApiResponse
            {
                Description = "Conflict. The request could not be completed due to a conflict with the current state of the resource.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 401 Unauthorized response for endpoints that require authorization
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Any();

            var hasAllowAnonymousAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (hasAuthorizeAttribute && !hasAllowAnonymousAttribute)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse
                {
                    Description = "Unauthorized. The client must authenticate itself to get the requested response.",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        [MediaTypeNames.Application.Json] = new OpenApiMediaType { Schema = problemDetailsSchema }
                    }
                });
            }
        }
    }
}