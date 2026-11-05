using Prothia.API.Analytics.Application.Errors;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Resources;
using Prothia.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Prothia.API.Analytics.Interfaces.Rest.Transform;

public static class ActionResultFromCreateClinicalReportResultAssembler
{
    public static ActionResult ToActionResultFromCreateClinicalReportResult(
        Result<ClinicalReport, ECreateClinicalReportError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getReportByIdActionName) =>
        result switch
        {
            Result<ClinicalReport, ECreateClinicalReportError>.Success success =>
                controller.CreatedAtAction(getReportByIdActionName, new { id = success.Value.Id },
                    ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<ClinicalReport, ECreateClinicalReportError>.Failure failure =>
                failure.Error switch
                {
                    ECreateClinicalReportError.PatientNotFound =>
                        controller.NotFound(localizer["AnalyticsPatientNotFound"].Value),
                    ECreateClinicalReportError.DuplicateReportCode =>
                        controller.Conflict(localizer["ClinicalReportDuplicated"].Value),
                    ECreateClinicalReportError.InvalidClinicalReport =>
                        controller.BadRequest(localizer["InvalidClinicalReportRequest"].Value),
                    ECreateClinicalReportError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingClinicalReport"].Value,
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
