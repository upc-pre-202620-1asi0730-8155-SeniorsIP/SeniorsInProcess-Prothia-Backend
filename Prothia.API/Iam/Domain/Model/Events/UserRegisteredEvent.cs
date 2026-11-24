using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Iam.Domain.Model.Events;

public sealed record UserRegisteredEvent(string Username) : IEvent;