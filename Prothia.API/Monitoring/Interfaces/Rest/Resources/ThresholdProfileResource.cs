namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record ThresholdProfileResource(
    int Id,
    int PatientId,
    int PelvicTilt,
    int Asymmetry,
    int Cadence,
    double ImpactPeak);