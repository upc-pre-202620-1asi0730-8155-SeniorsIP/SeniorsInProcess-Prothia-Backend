using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Monitoring.Domain.Model.Events;

public record BiomechanicalAlertRaisedEvent(
    AlertId AlertId,
    PatientId PatientId,
    string PatientName,
    string Type,
    string Severity) : IEvent;
