using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Interfaces.Rest.Resources;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ClinicalTransmissionResource from a ClinicalTransmission.
/// </summary>
public static class ClinicalTransmissionResourceFromEntityAssembler
{
    public static ClinicalTransmissionResource ToResourceFromEntity(ClinicalTransmission entity) =>
        new(
            entity.Id.Value,
            entity.PatientId.Value,
            entity.PatientName,
            entity.PatientDni,
            entity.WorkshopName.Value,
            entity.SharedModules,
            entity.TechnicalNotes,
            entity.Status.ToString().ToLowerInvariant(),
            entity.StatusLabel,
            entity.Summary,
            entity.CreatedAtDate);
}