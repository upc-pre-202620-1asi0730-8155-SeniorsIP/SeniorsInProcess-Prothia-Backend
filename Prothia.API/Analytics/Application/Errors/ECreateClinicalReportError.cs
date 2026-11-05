namespace Prothia.API.Analytics.Application.Errors;

public enum ECreateClinicalReportError
{
    InvalidClinicalReport,
    PatientNotFound,
    DuplicateReportCode,
    UnexpectedError
}