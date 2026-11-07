namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record AlertResource(
    int Id,
    int PatientId,
    string PatientName,
    string DeviationType,
    string RecordedValue,
    string Threshold,
    string Severity,
    string Status,
    DateTimeOffset OccurredAt,
    DateTimeOffset? ResolvedAt);