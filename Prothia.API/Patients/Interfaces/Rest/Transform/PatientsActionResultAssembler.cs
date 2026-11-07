using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Patients.Domain.Model;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public class PatientsActionResultAssembler
{
    
    private static int ToStatusCodeFromPatientsError(PatientsError error)
    {
        return error switch
        {
            PatientsError.PatientNotFound => StatusCodes.Status404NotFound,
            PatientsError.DniAlreadyRegistered => StatusCodes.Status409Conflict,
            PatientsError.InvalidPatientData => StatusCodes.Status400BadRequest,
            PatientsError.OperationCancelled => StatusCodes.Status409Conflict,
            PatientsError.DatabaseError => StatusCodes.Status500InternalServerError,
            PatientsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromCreatePatientResult(
        ControllerBase controller,
        Result<Patient> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Patient, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromPatientsError((PatientsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetPatientByIdResult(
        ControllerBase controller,
        Patient? patient,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Patient, IActionResult> successAction)
    {
        if (patient is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromPatientsError(PatientsError.PatientNotFound),
                PatientsError.PatientNotFound,
                errorLocalizer[$"PatientsError.{nameof(PatientsError.PatientNotFound)}"]
            );
        return successAction(patient);
    }
}