using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Prescriptions.Domain.Model;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

public static class PrescriptionsActionResultAssembler
{
    private static int ToStatusCodeFromPrescriptionsError(PrescriptionsError error)
    {
        return error switch
        {
            PrescriptionsError.PatientNotFound => StatusCodes.Status404NotFound,
            PrescriptionsError.ExerciseNotFound => StatusCodes.Status404NotFound,
            PrescriptionsError.DuplicateExerciseName => StatusCodes.Status409Conflict,
            PrescriptionsError.PrescriptionNotFound => StatusCodes.Status404NotFound,
            PrescriptionsError.InvalidPrescriptionData => StatusCodes.Status400BadRequest,
            PrescriptionsError.OperationCancelled => StatusCodes.Status409Conflict,
            PrescriptionsError.DatabaseError => StatusCodes.Status500InternalServerError,
            PrescriptionsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromExerciseResult(
        ControllerBase controller,
        Result<Exercise> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Exercise, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromPrescriptionsError((PrescriptionsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetExerciseByIdResult(
        ControllerBase controller,
        Exercise? exercise,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Exercise, IActionResult> successAction)
    {
        if (exercise is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromPrescriptionsError(PrescriptionsError.ExerciseNotFound),
                PrescriptionsError.ExerciseNotFound,
                errorLocalizer[$"PrescriptionsError.{nameof(PrescriptionsError.ExerciseNotFound)}"]
            );
        return successAction(exercise);
    }

    public static IActionResult ToActionResultFromPrescriptionResult(
        ControllerBase controller,
        Result<Prescription> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Prescription, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromPrescriptionsError((PrescriptionsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetPrescriptionByIdResult(
        ControllerBase controller,
        Prescription? prescription,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Prescription, IActionResult> successAction)
    {
        if (prescription is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromPrescriptionsError(PrescriptionsError.PrescriptionNotFound),
                PrescriptionsError.PrescriptionNotFound,
                errorLocalizer[$"PrescriptionsError.{nameof(PrescriptionsError.PrescriptionNotFound)}"]
            );
        return successAction(prescription);
    }
}
