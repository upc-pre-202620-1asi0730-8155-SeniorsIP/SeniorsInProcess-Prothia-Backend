using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Interfaces.Rest.Resources;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ClinicalMessageResource from a ClinicalMessage.
/// </summary>
public static class ClinicalMessageResourceFromEntityAssembler
{
    public static ClinicalMessageResource ToResourceFromEntity(ClinicalMessage entity) =>
        new(
            entity.Id.Value,
            entity.Sender.ToString().ToLowerInvariant(),
            entity.SenderName,
            entity.SenderRole,
            entity.Text.Value,
            entity.Timestamp,
            entity.IsSystem,
            entity.Status.ToString().ToLowerInvariant());
}