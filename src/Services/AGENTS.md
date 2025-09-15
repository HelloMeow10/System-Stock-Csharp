# Agent Instructions for the Services API

This document provides guidance for AI agents (and human developers) on how to maintain the conventions and architecture of this REST API project.

## 1. API Response Standards

This API follows modern REST best practices for responses.

### Success Responses
- For endpoints that return a single resource (e.g., `GET /api/v1/users/{id}`), the resource DTO should be returned directly with a `200 OK` status.
- For `POST` endpoints that create a resource, the newly created DTO should be returned directly with a `201 Created` status and a `Location` header pointing to the new resource.
- For endpoints that return a collection of resources, the response should be a `PagedResponse<T>` object, which includes pagination metadata and the collection of DTOs.
- **DO NOT** wrap success responses in any other custom object (like the old `ApiResponse`).

### Error Responses
- All error responses are handled automatically by the `GlobalExceptionHandler`.
- This handler produces a standard `ProblemDetails` (RFC 7807) response.
- To trigger an error response from a controller, **throw an appropriate exception**. Do not return `BadRequest()`, `NotFound()`, etc., directly.
- **Common Exceptions:**
  - `BusinessLogicException("Resource not found")`: The handler will convert this to a `404 Not Found`.
  - `AuthenticationException("Invalid credentials")`: The handler will convert this to a `401 Unauthorized`.
  - `ValidationException(errors)`: The handler will convert this to a `400 Bad Request` with a list of validation errors.
  - Any other `BusinessLogicException`: The handler will convert this to a `409 Conflict`.
  - Any other unhandled exception will result in a `500 Internal Server Error`.

## 2. HATEOAS Implementation

The API uses a generic, factory-based system to add HATEOAS links to responses.

### How it Works
- The `HateoasActionFilter` automatically runs on all successful `200 OK` responses.
- It inspects the response. If the response contains an object (or a collection of objects) that inherits from `SharedKernel.ResourceDto`, it attempts to add links.
- It uses dependency injection to find a corresponding `ILinkFactory<T>` for the DTO type.
- The factory is then responsible for adding the correct links to the DTO's `Links` property.

### How to Add HATEOAS to a New DTO
1. **Ensure the DTO inherits from `ResourceDto`**:
   ```csharp
   // src/Contracts/MyNewDto.cs
   public class MyNewDto : SharedKernel.ResourceDto
   {
       // ... properties
   }
   ```
2. **Create a Link Factory**:
   - Create a new class in `src/Services/Hateoas` that implements `ILinkFactory<MyNewDto>`.
   - Implement the `AddLinks` method to add all relevant links (e.g., `self`, `update`, `delete`). Use the `IUrlHelper` and named routes from the controller.
   ```csharp
   // src/Services/Hateoas/MyNewDtoLinksFactory.cs
   public class MyNewDtoLinksFactory : ILinkFactory<MyNewDto>
   {
       public void AddLinks(MyNewDto resource, IUrlHelper urlHelper)
       {
           resource.Links.Add(new LinkDto(
               href: urlHelper.Link("GetMyNewDtoById", new { id = resource.Id }),
               rel: "self",
               method: "GET"
           ));
           // ... add other links
       }
   }
   ```
3. **Register the Factory**:
   - In `src/Services/ServiceCollectionExtensions.cs`, add the registration for your new factory inside the `AddApiServices` method.
   ```csharp
   // src/Services/ServiceCollectionExtensions.cs
   services.AddScoped<ILinkFactory<MyNewDto>, MyNewDtoLinksFactory>();
   ```

That's it. The `HateoasActionFilter` will now automatically find and use your factory for `MyNewDto` objects.
