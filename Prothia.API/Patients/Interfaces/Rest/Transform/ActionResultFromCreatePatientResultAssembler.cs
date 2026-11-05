using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Patients.Application.Errors;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public static class ActionResultFromCreatePatientResultAssembler
{
    public static ActionResult ToActionResultFromCreatePatientResult(
        Result<Patient, ECreatePatientError> result,
        ControllerBase controller,
        IStringLocalizer<ErrorMessages> localizer,
        string getPatientByIdActionName) =>
        result switch
        {
            Result<Patient, ECreatePatientError>.Success success =>
                controller.CreatedAtAction(getPatientByIdActionName, new { id = success.Value.Id },
                    PatientResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Patient, ECreatePatientError>.Failure failure =>
                failure.Error switch
                {
                    ECreatePatientError.InvalidPatient =>
                        controller.BadRequest(localizer["InvalidPatientRequest"].Value),

                    ECreatePatientError.DuplicatePatient =>
                        controller.Conflict(localizer["PatientsPatientDuplicated"].Value),

                    ECreatePatientError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingPatient"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
}
