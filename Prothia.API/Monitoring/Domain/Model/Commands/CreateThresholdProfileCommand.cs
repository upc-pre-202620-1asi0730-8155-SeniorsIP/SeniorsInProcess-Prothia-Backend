using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Commands;

public record CreateThresholdProfileCommand(
    PatientId PatientId,
    int PelvicTilt,
    int Asymmetry,
    int Cadence,
    double ImpactPeak);