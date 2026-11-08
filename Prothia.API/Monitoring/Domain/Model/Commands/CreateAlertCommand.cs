using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Commands;

public record CreateAlertCommand(
    PatientId PatientId,
    DeviationType  DeviationType,
    ReadingText RecordedValue,
    ReadingText Threshold,
    EAlertSeverity Severity,
    DateTimeOffset OccurredAt);