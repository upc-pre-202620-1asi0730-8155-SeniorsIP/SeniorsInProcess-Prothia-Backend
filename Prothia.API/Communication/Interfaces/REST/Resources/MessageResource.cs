namespace Prothia.API.Communication.Interfaces.REST.Resources;

public record MessageResource(Guid Id, Guid SenderUserId, Guid RecipientUserId, string Body, DateTime SentAt);