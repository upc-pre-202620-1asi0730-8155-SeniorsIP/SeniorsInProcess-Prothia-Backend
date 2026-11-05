namespace Prothia.API.Patients.Application.Internal.EventHandlers;

public class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    : IEventHandler<UserRegisteredEvent>
{
    
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing UserRegisteredEvent for User ID {UserId} ({Username}) in Patients context",
            domainEvent.UserId.Value, domainEvent.Username);

        return Task.CompletedTask;
    }
}
