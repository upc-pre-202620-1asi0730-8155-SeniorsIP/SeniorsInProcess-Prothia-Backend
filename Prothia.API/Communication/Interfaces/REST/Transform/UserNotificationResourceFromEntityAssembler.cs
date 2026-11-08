using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Interfaces.REST.Resources;

namespace Prothia.API.Communication.Interfaces.REST.Transform;

public static class UserNotificationResourceFromEntityAssembler
{
    public static UserNotificationResource ToResourceFromEntity(UserNotification entity)
    {
        // Convertimos el Enum 'Type' a string para que el JSON sea más legible
        return new UserNotificationResource(entity.Id, entity.UserId, entity.Type.ToString(), entity.ReferenceId, entity.ReadAt);
    }
}