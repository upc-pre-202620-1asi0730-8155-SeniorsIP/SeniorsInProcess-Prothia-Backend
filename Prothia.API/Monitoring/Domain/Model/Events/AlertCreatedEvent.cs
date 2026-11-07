using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Events;

public sealed record AlertCreatedEvent(int PatientId, string DeviationType, EAlertSeverity Severity) : IEvent;