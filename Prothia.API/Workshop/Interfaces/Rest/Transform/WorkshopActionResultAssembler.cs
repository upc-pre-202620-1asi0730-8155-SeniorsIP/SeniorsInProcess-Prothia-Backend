using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;
using Prothia.API.Workshop.Domain.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class WorkshopActionResultAssembler
{
    private static int ToStatusCodeFromWorkshopError(WorkshopError error)
    {
        return error switch
        {
            WorkshopError.ProsthesisNotFound => StatusCodes.Status404NotFound,
            WorkshopError.DuplicateSerialNumber => StatusCodes.Status409Conflict,
            WorkshopError.MechanicalAlertNotFound => StatusCodes.Status404NotFound,
            WorkshopError.MechanicalAlertAlreadyResolved => StatusCodes.Status409Conflict,
            WorkshopError.MaintenanceScheduleNotFound => StatusCodes.Status404NotFound,
            WorkshopError.InvalidWorkshopData => StatusCodes.Status400BadRequest,
            WorkshopError.OperationCancelled => StatusCodes.Status409Conflict,
            WorkshopError.DatabaseError => StatusCodes.Status500InternalServerError,
            WorkshopError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromProsthesisResult(
        ControllerBase controller,
        Result<Prosthesis> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Prosthesis, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromWorkshopError((WorkshopError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetProsthesisByIdResult(
        ControllerBase controller,
        Prosthesis? prosthesis,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Prosthesis, IActionResult> successAction)
    {
        if (prosthesis is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromWorkshopError(WorkshopError.ProsthesisNotFound),
                WorkshopError.ProsthesisNotFound,
                errorLocalizer[$"WorkshopError.{nameof(WorkshopError.ProsthesisNotFound)}"]
            );
        return successAction(prosthesis);
    }

    public static IActionResult ToActionResultFromMechanicalAlertResult(
        ControllerBase controller,
        Result<MechanicalAlerts> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MechanicalAlerts, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromWorkshopError((WorkshopError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetMechanicalAlertByIdResult(
        ControllerBase controller,
        MechanicalAlerts? alert,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MechanicalAlerts, IActionResult> successAction)
    {
        if (alert is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromWorkshopError(WorkshopError.MechanicalAlertNotFound),
                WorkshopError.MechanicalAlertNotFound,
                errorLocalizer[$"WorkshopError.{nameof(WorkshopError.MechanicalAlertNotFound)}"]
            );
        return successAction(alert);
    }

    public static IActionResult ToActionResultFromMaintenanceScheduleResult(
        ControllerBase controller,
        Result<MaintenanceSchedule> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MaintenanceSchedule, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromWorkshopError((WorkshopError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetMaintenanceScheduleByIdResult(
        ControllerBase controller,
        MaintenanceSchedule? schedule,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MaintenanceSchedule, IActionResult> successAction)
    {
        if (schedule is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromWorkshopError(WorkshopError.MaintenanceScheduleNotFound),
                WorkshopError.MaintenanceScheduleNotFound,
                errorLocalizer[$"WorkshopError.{nameof(WorkshopError.MaintenanceScheduleNotFound)}"]
            );
        return successAction(schedule);
    }
}
