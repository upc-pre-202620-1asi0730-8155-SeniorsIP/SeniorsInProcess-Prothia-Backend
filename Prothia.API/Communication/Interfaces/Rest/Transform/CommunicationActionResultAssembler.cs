using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Communication.Domain.Model;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

public static class CommunicationActionResultAssembler
{
    private static int ToStatusCodeFromCommunicationError(CommunicationError error)
    {
        return error switch
        {
            CommunicationError.PatientNotFound => StatusCodes.Status404NotFound,
            CommunicationError.ClinicalMessageNotFound => StatusCodes.Status404NotFound,
            CommunicationError.ClinicalTransmissionNotFound => StatusCodes.Status404NotFound,
            CommunicationError.InvalidCommunicationData => StatusCodes.Status400BadRequest,
            CommunicationError.OperationCancelled => StatusCodes.Status409Conflict,
            CommunicationError.DatabaseError => StatusCodes.Status500InternalServerError,
            CommunicationError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromClinicalMessageResult(
        ControllerBase controller,
        Result<ClinicalMessage> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalMessage, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromCommunicationError((CommunicationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetClinicalMessageByIdResult(
        ControllerBase controller,
        ClinicalMessage? message,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalMessage, IActionResult> successAction)
    {
        if (message is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromCommunicationError(CommunicationError.ClinicalMessageNotFound),
                CommunicationError.ClinicalMessageNotFound,
                errorLocalizer[$"CommunicationError.{nameof(CommunicationError.ClinicalMessageNotFound)}"]
            );
        return successAction(message);
    }

    public static IActionResult ToActionResultFromClinicalTransmissionResult(
        ControllerBase controller,
        Result<ClinicalTransmission> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalTransmission, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromCommunicationError((CommunicationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetClinicalTransmissionByIdResult(
        ControllerBase controller,
        ClinicalTransmission? transmission,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalTransmission, IActionResult> successAction)
    {
        if (transmission is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromCommunicationError(CommunicationError.ClinicalTransmissionNotFound),
                CommunicationError.ClinicalTransmissionNotFound,
                errorLocalizer[$"CommunicationError.{nameof(CommunicationError.ClinicalTransmissionNotFound)}"]
            );
        return successAction(transmission);
    }
}
