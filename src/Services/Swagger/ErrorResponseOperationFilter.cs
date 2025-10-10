using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Services.Swagger
{
    /// <summary>
    /// An operation filter to add standard error responses (400, 401, 404, 409) to all operations.
    /// This avoids having to add [ProducesResponseType] attributes for these common errors on every action.
    /// </summary>
    public class ErrorResponseOperationFilter : IOperationFilter
    {
        private static readonly string ProblemDetailsContentType = "application/problem+json";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var existingErrorResponses = operation.Responses
                .Where(r => r.Key.StartsWith("4"))
                .Select(r => r.Key)
                .ToHashSet();

            // Check if the endpoint allows anonymous access
            var hasAllowAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata
                                         .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            AddResponse(operation, "400", "Bad Request", existingErrorResponses);

            // Only add 401 if the endpoint does not allow anonymous access and the response doesn't already exist
            if (!hasAllowAnonymous)
            {
                AddResponse(operation, "401", "Unauthorized", existingErrorResponses);
            }

            AddResponse(operation, "404", "Not Found", existingErrorResponses);
            AddResponse(operation, "409", "Conflict", existingErrorResponses);
        }

        private void AddResponse(OpenApiOperation operation, string statusCode, string description, ISet<string> existingResponses)
        {
            if (existingResponses.Contains(statusCode))
            {
                return; // Don't add if a response for this code already exists
            }

            operation.Responses.Add(statusCode, new OpenApiResponse
            {
                Description = description,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [ProblemDetailsContentType] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = nameof(ProblemDetails)
                            }
                        }
                    }
                }
            });
        }
    }
}