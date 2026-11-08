using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Interfaces.REST.Resources;

namespace Prothia.API.Communication.Interfaces.REST.Transform;

public static class PublishNotificationCommandFromResourceAssembler
{
    public static PublishNotificationCommand ToCommandFromResource(PublishNotificationResource resource)
    {
        return new PublishNotificationCommand(resource.UserId, resource.Type, resource.ReferenceId);
    }
}