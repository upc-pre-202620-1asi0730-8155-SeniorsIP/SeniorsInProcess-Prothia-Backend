using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.Commands;

public record PublishNotificationCommand(Guid UserId, NotificationType Type, Guid? ReferenceId);