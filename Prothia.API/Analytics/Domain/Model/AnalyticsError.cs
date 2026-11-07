namespace Prothia.API.Analytics.Domain.Model;

public enum AnalyticsError
{
    None,
    PatientNotFound,
    ClinicalReportNotFound,
    DuplicateReportCode,
    InvalidClinicalReportData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}