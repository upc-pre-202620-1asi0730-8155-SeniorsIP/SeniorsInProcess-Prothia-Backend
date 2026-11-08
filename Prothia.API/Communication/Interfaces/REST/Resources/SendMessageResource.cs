namespace Prothia.API.Communication.Interfaces.REST.Resources;

public record SendMessageResource(Guid SenderUserId, Guid RecipientUserId, string Body);