namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record GaitSessionResource(
    int Id,
    int PatientId,
    DateTimeOffset RecordedAt,
    int DurationMinutes,
    int Steps,
    int Cadence,
    int SymmetryPercent,
    string SymmetryGrade,
    int AlertsCount,
    int ImpactPeakNewtons);