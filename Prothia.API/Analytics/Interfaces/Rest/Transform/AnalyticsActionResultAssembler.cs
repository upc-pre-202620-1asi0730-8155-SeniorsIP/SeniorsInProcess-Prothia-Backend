using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Analytics.Domain.Model;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Analytics.Interfaces.Rest.Transform;

public static class AnalyticsActionResultAssembler
{
    private static int ToStatusCodeFromAnalyticsError(AnalyticsError error)
    {
        return error switch
        {
            AnalyticsError.PatientNotFound => StatusCodes.Status404NotFound,
            AnalyticsError.ClinicalReportNotFound => StatusCodes.Status404NotFound,
            AnalyticsError.DuplicateReportCode => StatusCodes.Status409Conflict,
            AnalyticsError.InvalidClinicalReportData => StatusCodes.Status400BadRequest,
            AnalyticsError.OperationCancelled => StatusCodes.Status409Conflict,
            AnalyticsError.DatabaseError => StatusCodes.Status500InternalServerError,
            AnalyticsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromClinicalReportResult(
        ControllerBase controller,
        Result<ClinicalReport> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalReport, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetClinicalReportByIdResult(
        ControllerBase controller,
        ClinicalReport? report,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicalReport, IActionResult> successAction)
    {
        if (report is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromAnalyticsError(AnalyticsError.ClinicalReportNotFound),
                AnalyticsError.ClinicalReportNotFound,
                errorLocalizer[$"AnalyticsError.{nameof(AnalyticsError.ClinicalReportNotFound)}"]
            );
        return successAction(report);
    }
}
