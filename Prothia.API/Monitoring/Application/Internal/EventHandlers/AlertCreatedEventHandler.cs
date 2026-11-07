using Prothia.API.Monitoring.Domain.Model.Events;

namespace Prothia.API.Monitoring.Application.Internal.EventHandlers;

public class AlertCreatedEventHandler(ILogger<AlertCreatedEventHandler> logger)
    : IEventHandler<AlertCreatedEvent>
{
    public Task Handle(AlertCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processed AlertCreatedEvent for PatientId {PatientId} (Deviation: {DeviationType}, Severity: {Severity})",
            domainEvent.PatientId, domainEvent.DeviationType, domainEvent.Severity);
        return Task.CompletedTask;
    }
}