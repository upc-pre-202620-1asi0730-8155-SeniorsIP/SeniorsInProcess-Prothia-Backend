using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Monitoring.Domain.Model;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public class MonitoringActionResultAssembler
{
    private static int ToStatusCodeFromMonitoringError(MonitoringError error)
    {
        return error switch
        {
            MonitoringError.PatientNotFound => StatusCodes.Status404NotFound,
            MonitoringError.AlertNotFound => StatusCodes.Status404NotFound,
            MonitoringError.AlertAlreadyResolved => StatusCodes.Status409Conflict,
            MonitoringError.ThresholdProfileNotFound => StatusCodes.Status404NotFound,
            MonitoringError.DuplicateThresholdProfileForPatient => StatusCodes.Status409Conflict,
            MonitoringError.GaitSessionNotFound => StatusCodes.Status404NotFound,
            MonitoringError.InvalidMonitoringData => StatusCodes.Status400BadRequest,
            MonitoringError.OperationCancelled => StatusCodes.Status409Conflict,
            MonitoringError.DatabaseError => StatusCodes.Status500InternalServerError,
            MonitoringError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromAlertResult(
        ControllerBase controller,
        Result<Alert> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Alert, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMonitoringError((MonitoringError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetAlertByIdResult(
        ControllerBase controller,
        Alert? alert,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Alert, IActionResult> successAction)
    {
        if (alert is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMonitoringError(MonitoringError.AlertNotFound),
                MonitoringError.AlertNotFound,
                errorLocalizer[$"MonitoringError.{nameof(MonitoringError.AlertNotFound)}"]
            );
        return successAction(alert);
    }

    public static IActionResult ToActionResultFromThresholdProfileResult(
        ControllerBase controller,
        Result<ThresholdProfile> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ThresholdProfile, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMonitoringError((MonitoringError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetThresholdProfileByIdResult(
        ControllerBase controller,
        ThresholdProfile? profile,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ThresholdProfile, IActionResult> successAction)
    {
        if (profile is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMonitoringError(MonitoringError.ThresholdProfileNotFound),
                MonitoringError.ThresholdProfileNotFound,
                errorLocalizer[$"MonitoringError.{nameof(MonitoringError.ThresholdProfileNotFound)}"]
            );
        return successAction(profile);
    }

    public static IActionResult ToActionResultFromGaitSessionResult(
        ControllerBase controller,
        Result<GaitSession> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<GaitSession, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMonitoringError((MonitoringError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetGaitSessionByIdResult(
        ControllerBase controller,
        GaitSession? session,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<GaitSession, IActionResult> successAction)
    {
        if (session is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMonitoringError(MonitoringError.GaitSessionNotFound),
                MonitoringError.GaitSessionNotFound,
                errorLocalizer[$"MonitoringError.{nameof(MonitoringError.GaitSessionNotFound)}"]
            );
        return successAction(session);
    }
}