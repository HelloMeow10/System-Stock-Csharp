using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Services.Swagger
{
    /// <summary>
    /// Adds standardized error response documentation to Swagger/OpenAPI for API operations.
    /// </summary>
    public class ErrorResponseOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to add standard error responses to the operation's documentation.
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var problemDetailsSchema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

            // Add 400 Bad Request
            operation.Responses.TryAdd("400", new OpenApiResponse
            {
                Description = "Bad Request - The server could not understand the request due to invalid syntax or validation errors.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 404 Not Found
            operation.Responses.TryAdd("404", new OpenApiResponse
            {
                Description = "Not Found - The requested resource could not be found.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 409 Conflict
            operation.Responses.TryAdd("409", new OpenApiResponse
            {
                Description = "Conflict - The request could not be completed due to a conflict with the current state of the resource.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType { Schema = problemDetailsSchema }
                }
            });

            // Add 401 Unauthorized, unless the endpoint allows anonymous access
            var hasAllowAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata
                                           .Any(em => em is AllowAnonymousAttribute);
            if (!hasAllowAnonymous)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse
                {
                    Description = "Unauthorized - The client must authenticate itself to get the requested response.",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/problem+json"] = new OpenApiMediaType { Schema = problemDetailsSchema }
                    }
                });
            }
        }
    }
}