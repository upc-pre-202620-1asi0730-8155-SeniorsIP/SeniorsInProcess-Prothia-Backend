namespace Prothia.API.Analytics.Interfaces.Rest.Resources;

public record ClinicalReportResource(
    int Id,
    string ReportCode,
    string IssueDate,
    string ClinicName,
    string ClinicUnit,
    int PatientId,
    string PatientName,
    string Dni,
    int Age,
    string Diagnosis,
    string ProsthesisCode,
    string EvaluationPeriod,
    string Summary,
    string MedicalRecommendation,
    string TherapistName,
    string TherapistRole,
    string TherapistRegistry,
    IEnumerable<ReportIndicatorResource> Indicators);