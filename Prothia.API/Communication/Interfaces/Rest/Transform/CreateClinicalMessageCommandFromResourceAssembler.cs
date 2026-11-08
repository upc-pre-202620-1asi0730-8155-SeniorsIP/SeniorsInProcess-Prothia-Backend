using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Communication.Interfaces.Rest.Resources;
using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateClinicalMessageCommand from a CreateClinicalMessageResource.
/// </summary>
public static class CreateClinicalMessageCommandFromResourceAssembler
{
    public static CreateClinicalMessageCommand ToCommandFromResource(CreateClinicalMessageResource resource) =>
        new(
            EnumConversions.Parse<MessageSender>(resource.Sender, nameof(resource.Sender)),
            resource.SenderName,
            resource.SenderRole,
            new MessageText(resource.Text),
            resource.Timestamp ?? DateTimeOffset.UtcNow.ToString("HH:mm"),
            resource.IsSystem,
            string.IsNullOrWhiteSpace(resource.Status)
                ? MessageDeliveryStatus.Sent
                : EnumConversions.Parse<MessageDeliveryStatus>(resource.Status, nameof(resource.Status)));
}