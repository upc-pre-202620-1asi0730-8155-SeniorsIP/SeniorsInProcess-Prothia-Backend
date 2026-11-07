using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record CreateAlertResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidPatientId")]
    int PatientId,
    [Required(ErrorMessage = "RequiredDeviationType")]
    string DeviationType,
    [Required(ErrorMessage = "RequiredRecordedValue")]
    string RecordedValue,
    [Required(ErrorMessage = "RequiredAlertThreshold")]
    string Threshold,
    [Required(ErrorMessage = "RequiredSeverity")]
    string Severity,
    DateTimeOffset? OccurredAt = null);