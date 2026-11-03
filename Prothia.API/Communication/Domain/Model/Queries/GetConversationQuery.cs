namespace Prothia.API.Communication.Domain.Model.Queries;

public record GetConversationQuery(Guid UserId, Guid OtherUserId);