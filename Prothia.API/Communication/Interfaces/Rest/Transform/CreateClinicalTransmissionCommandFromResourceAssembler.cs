using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Communication.Interfaces.Rest.Resources;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateClinicalTransmissionCommand from a CreateClinicalTransmissionResource.
/// </summary>
public static class CreateClinicalTransmissionCommandFromResourceAssembler
{
    public static CreateClinicalTransmissionCommand ToCommandFromResource(CreateClinicalTransmissionResource resource) =>
        new(
            new PatientId(resource.PatientId),
            resource.PatientDni,
            new WorkshopName(resource.WorkshopName),
            resource.SharedModules ?? [],
            resource.TechnicalNotes ?? string.Empty,
            resource.Summary ?? string.Empty,
            resource.CreatedAtDate ?? DateTimeOffset.UtcNow.ToString("yyyy-MM-dd"));
}