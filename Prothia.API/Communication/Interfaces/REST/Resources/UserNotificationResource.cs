namespace Prothia.API.Communication.Interfaces.REST.Resources;

public record UserNotificationResource(Guid Id, Guid UserId, string Type, Guid? ReferenceId, DateTime? ReadAt);