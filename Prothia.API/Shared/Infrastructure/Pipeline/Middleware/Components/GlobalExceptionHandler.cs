using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Global exception handler implementing ASP.NET Core <see cref="IExceptionHandler" />.
/// </summary>
/// <remarks>
///     Catches unhandled exceptions and writes RFC 9457 compliant Problem Details responses.
/// </remarks>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ErrorMessages> localizer)
    : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException opEx)
        {
            logger.LogWarning(opEx, "Request was cancelled: {Message}", opEx.Message);
            var cancelledProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = localizer["OperationCancelledTitle"].Value,
                Detail = localizer["OperationCancelledDetail"].Value,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = cancelledProblemDetails,
                Exception = exception
            });
        }

        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var genericProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = localizer["UnexpectedServerError"].Value,
            Detail = localizer["UnexpectedErrorProcessingRequest"].Value,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = genericProblemDetails,
            Exception = exception
        });
    }
}
