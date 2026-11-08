using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Interfaces.REST.Resources;

namespace Prothia.API.Communication.Interfaces.REST.Transform;

public static class SendMessageCommandFromResourceAssembler
{
    public static SendMessageCommand ToCommandFromResource(SendMessageResource resource)
    {
        return new SendMessageCommand(resource.SenderUserId, resource.RecipientUserId, resource.Body);
    }
}