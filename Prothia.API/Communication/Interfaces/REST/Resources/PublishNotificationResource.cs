using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Interfaces.REST.Resources;

public record PublishNotificationResource(Guid UserId, NotificationType Type, Guid? ReferenceId);