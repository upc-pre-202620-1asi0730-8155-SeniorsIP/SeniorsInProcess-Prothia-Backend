namespace Prothia.API.Communication.Domain.Model.Commands;

public record SendMessageCommand(Guid SenderUserId, Guid RecipientUserId, string Body);