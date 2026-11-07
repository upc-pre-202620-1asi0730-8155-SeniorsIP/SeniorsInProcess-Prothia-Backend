using System.ComponentModel.DataAnnotations;
namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record RecordGaitSessionResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidPatientId")]
    int PatientId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidDurationMinutes")]
    int DurationMinutes,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidSteps")]
    int Steps,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidGaitCadence")]
    int Cadence,
    [Range(0, 100, ErrorMessage = "InvalidSymmetryPercent")]
    int SymmetryPercent,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidAlertsCount")]
    int AlertsCount,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidImpactPeakNewtons")]
    int ImpactPeakNewtons,
    DateTimeOffset? RecordedAt = null);