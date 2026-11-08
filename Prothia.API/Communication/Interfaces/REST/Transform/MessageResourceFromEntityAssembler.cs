using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Interfaces.REST.Resources;

namespace Prothia.API.Communication.Interfaces.REST.Transform;

public static class MessageResourceFromEntityAssembler
{
    public static MessageResource ToResourceFromEntity(Message entity)
    {
        return new MessageResource(entity.Id, entity.SenderUserId, entity.RecipientUserId, entity.Body, entity.SentAt);
    }
}