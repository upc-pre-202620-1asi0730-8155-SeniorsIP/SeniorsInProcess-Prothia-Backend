namespace Prothia.API.Patients.Application.Internal.EventHandlers;

public class PatientRegisteredEventHandler(ILogger<PatientRegisteredEventHandler> logger)
    : IEventHandler<PatientRegisteredEvent>
{
    public Task Handle(PatientRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processed PatientRegisteredEvent for Patient {FullName} (DNI: {MaskedDni}, KLevel: {KLevel})",
            domainEvent.FullName, domainEvent.MaskedDni, domainEvent.KLevel);
        return Task.CompletedTask;
    }
}