using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.Commands;

public record CreateClinicalReportCommand(
    ReportCode ReportCode,
    string IssueDate,
    string ClinicName,
    string ClinicUnit,
    PatientId PatientId,
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
    TherapistRegistry TherapistRegistry,
    IReadOnlyCollection<ReportIndicatorItem> Indicators);