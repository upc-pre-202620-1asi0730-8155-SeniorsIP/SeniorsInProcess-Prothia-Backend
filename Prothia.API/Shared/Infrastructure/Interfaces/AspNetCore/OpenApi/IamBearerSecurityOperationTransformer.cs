using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;

public sealed class IamBearerSecurityOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        if (metadata.OfType<AllowAnonymousAttribute>().Any() ||
            !metadata.OfType<AuthorizeAttribute>().Any())
        {
            return Task.CompletedTask;
        }

        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
        });

        return Task.CompletedTask;
    }
}