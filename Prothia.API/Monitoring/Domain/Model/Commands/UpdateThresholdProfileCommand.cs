namespace Prothia.API.Monitoring.Domain.Model.Commands;

public record UpdateThresholdProfileCommand(
    int ThresholdProfileId,
    int PelvicTilt,
    int Asymmetry,
    int Cadence,
    double ImpactPeak);