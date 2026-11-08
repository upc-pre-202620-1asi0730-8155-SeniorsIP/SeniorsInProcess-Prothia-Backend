using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Monitoring.Domain.Model.Events;

public sealed record AlertCreatedEvent(int PatientId, string DeviationType, EAlertSeverity Severity) : IEvent;