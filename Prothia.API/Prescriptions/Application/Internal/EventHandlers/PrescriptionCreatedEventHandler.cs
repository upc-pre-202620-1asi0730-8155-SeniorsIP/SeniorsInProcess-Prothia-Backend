using Prothia.API.Prescriptions.Domain.Model.Events;
using Prothia.API.Shared.Application.Internal.EventHandlers;

namespace Prothia.API.Prescriptions.Application.Internal.EventHandlers;

public class PrescriptionCreatedEventHandler(ILogger<PrescriptionCreatedEventHandler> logger)
    : IEventHandler<PrescriptionCreatedEvent>
{
    public Task Handle(PrescriptionCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Prescription created for Patient ID {PatientId} (PrescriptionId: {PrescriptionId}, Phase {PhaseNumber}: {PhaseName})",
            domainEvent.PatientId.Value,
            domainEvent.PrescriptionId.Value,
            domainEvent.PhaseNumber,
            domainEvent.PhaseName);
        return Task.CompletedTask;
    }
}