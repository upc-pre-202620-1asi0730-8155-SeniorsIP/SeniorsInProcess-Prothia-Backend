using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Commands;

public record RecordGaitSessionCommand(
    PatientId PatientId,
    DateTimeOffset RecordedAt,
    int DurationMinutes,
    int Steps,
    int Cadence,
    int SymmetryPercent,
    int AlertsCount,
    int ImpactPeakNewtons
    );