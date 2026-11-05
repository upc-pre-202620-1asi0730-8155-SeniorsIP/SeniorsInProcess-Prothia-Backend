using Prothia.API.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace Prothia.API.Shared.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Middleware extensions
/// </summary>
public static class MiddlewareExtensions
{
    [Obsolete("Use app.UseExceptionHandler() with AddExceptionHandler<GlobalExceptionHandler>() instead.")]
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}