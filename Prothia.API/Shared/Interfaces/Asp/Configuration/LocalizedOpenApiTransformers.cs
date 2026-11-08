using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using Prothia.API.Shared.Resources;

namespace Prothia.API.Shared.Interfaces.Asp.Configuration;

/// <summary>
///     Resource keys documenting one controller action: summary, description and one text per HTTP status code.
/// </summary>
/// <param name="Summary">Resource key of the operation summary.</param>
/// <param name="Description">Resource key of the operation description.</param>
/// <param name="Responses">Resource key of the description of each response, keyed by HTTP status code.</param>
public sealed record OperationDocumentation(
    string Summary,
    string Description,
    IReadOnlyDictionary<string, string> Responses);

/// <summary>
///     Reusable engine that bounded contexts use to localize their OpenAPI metadata.
///     Each context owns its own tables of resource keys and delegates the transformation to this class.
/// </summary>
public static class LocalizedOpenApiTransformers
{
    /// <summary>
    ///     Builds the documentation keys of an action following the naming convention
    ///     <c>{Action}Summary</c>, <c>{Action}Description</c> and <c>{Action}{ResponseSuffix}</c>.
    /// </summary>
    /// <param name="action">The controller action name.</param>
    /// <param name="responses">Pairs of HTTP status code and resource key suffix.</param>
    public static OperationDocumentation Operation(string action, params (string StatusCode, string Suffix)[] responses) =>
        new($"{action}Summary", $"{action}Description",
            responses.ToDictionary(r => r.StatusCode, r => $"{action}{r.Suffix}"));

    /// <summary>
    ///     Localizes summary, description, operation id and response descriptions of the known operations.
    /// </summary>
    public static Task TransformOperationAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        IReadOnlyDictionary<string, OperationDocumentation> operations)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action ||
            !operations.TryGetValue(action.ActionName, out var documentation))
            return Task.CompletedTask;

        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<CommonMessages>>();
        operation.OperationId = action.ActionName;
        operation.Summary = localizer[documentation.Summary].Value;
        operation.Description = localizer[documentation.Description].Value;
        if (operation.Responses is null) return Task.CompletedTask;

        foreach (var (statusCode, resourceKey) in documentation.Responses)
            if (operation.Responses.TryGetValue(statusCode, out var response) &&
                response is OpenApiResponse openApiResponse)
                openApiResponse.Description = localizer[resourceKey].Value;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Localizes the description of known resource schemas and of their properties.
    ///     Properties are identified by declaring type and JSON name, so two resources can both expose an <c>id</c>.
    /// </summary>
    public static Task TransformSchemaAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        IReadOnlyDictionary<Type, string> schemas,
        IReadOnlyDictionary<(Type DeclaringType, string Property), string> properties)
    {
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<CommonMessages>>();

        if (schemas.TryGetValue(context.JsonTypeInfo.Type, out var schemaKey))
            schema.Description = localizer[schemaKey].Value;

        var property = context.JsonPropertyInfo;
        if (property?.DeclaringType is { } declaringType &&
            properties.TryGetValue((declaringType, property.Name), out var propertyKey))
            schema.Description = localizer[propertyKey].Value;

        return Task.CompletedTask;
    }
}