using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Analytics.Interfaces.Rest.Resources;

public record CreateClinicalReportResource(
    [Required(ErrorMessage = "RequiredReportCode")]
    string ReportCode,
    [Required(ErrorMessage = "RequiredIssueDate")]
    string IssueDate,
    [Required(ErrorMessage = "RequiredClinicName")]
    string ClinicName,
    [Required(ErrorMessage = "RequiredClinicUnit")]
    string ClinicUnit,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidPatientId")]
    int PatientId,
    string? PatientName,
    [Required(ErrorMessage = "RequiredDni")]
    string Dni,
    [Range(1, 120, ErrorMessage = "InvalidAge")]
    int Age,
    [Required(ErrorMessage = "RequiredDiagnosis")]
    string Diagnosis,
    [Required(ErrorMessage = "RequiredProsthesisCode")]
    string ProsthesisCode,
    [Required(ErrorMessage = "RequiredEvaluationPeriod")]
    string EvaluationPeriod,
    [Required(ErrorMessage = "RequiredSummary")]
    string Summary,
    [Required(ErrorMessage = "RequiredMedicalRecommendation")]
    string MedicalRecommendation,
    [Required(ErrorMessage = "RequiredTherapistName")]
    string TherapistName,
    [Required(ErrorMessage = "RequiredTherapistRole")]
    string TherapistRole,
    [Required(ErrorMessage = "RequiredTherapistRegistry")]
    string TherapistRegistry,
    IEnumerable<ReportIndicatorResource>? Indicators = null);