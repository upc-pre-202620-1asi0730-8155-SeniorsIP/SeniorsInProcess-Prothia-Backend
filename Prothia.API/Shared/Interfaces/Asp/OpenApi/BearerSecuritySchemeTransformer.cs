using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Prothia.API.Shared.Interfaces.Asp.OpenApi;

/// <summary>
///     Adds the JWT Bearer security scheme definition to the OpenAPI document for Scalar interactive authorization.
/// </summary>
public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter the IAM JWT Bearer token."
        };

        return Task.CompletedTask;
    }
}