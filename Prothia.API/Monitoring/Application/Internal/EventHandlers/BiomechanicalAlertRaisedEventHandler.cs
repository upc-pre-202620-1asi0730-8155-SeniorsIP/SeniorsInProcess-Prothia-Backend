using Prothia.API.Monitoring.Domain.Model.Events;
using Prothia.API.Shared.Application.Internal.EventHandlers;

namespace Prothia.API.Monitoring.Application.Internal.EventHandlers;

public class BiomechanicalAlertRaisedEventHandler(ILogger<BiomechanicalAlertRaisedEventHandler> logger)
    : IEventHandler<BiomechanicalAlertRaisedEvent>
{
    public Task Handle(BiomechanicalAlertRaisedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Biomechanical alert raised for patient {PatientName} (PatientId: {PatientId}, AlertId: {AlertId}, Type: {Type}, Severity: {Severity})",
            domainEvent.PatientName,
            domainEvent.PatientId.Value,
            domainEvent.AlertId.Value,
            domainEvent.Type,
            domainEvent.Severity);
        return Task.CompletedTask;
    }
}