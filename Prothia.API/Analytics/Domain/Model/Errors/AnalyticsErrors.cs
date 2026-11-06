using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Analytics.Domain.Model.Errors;

public static class AnalyticsErrors
{
    public static readonly Error PatientNotFound = Error.NotFound(
        "Analytics.PatientNotFound",
        "The specified patient was not found.");

    public static readonly Error ClinicalReportNotFound = Error.NotFound(
        "Analytics.ClinicalReportNotFound",
        "The specified clinical report was not found.");

    public static readonly Error DuplicateReportCode = Error.Conflict(
        "Analytics.DuplicateReportCode",
        "A clinical report with the specified report code already exists.");

    public static readonly Error InvalidClinicalReportData = Error.Validation(
        "Analytics.InvalidClinicalReportData",
        "The provided clinical report data is invalid.");
}