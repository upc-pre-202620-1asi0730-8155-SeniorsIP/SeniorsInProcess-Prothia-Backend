using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public static class CreateThresholdProfileCommandFromResourceAssembler
{
    public static CreateThresholdProfileCommand ToCommandFromResource(CreateThresholdProfileResource resource) =>
        new(new PatientId(resource.PatientId), resource.PelvicTilt, resource.Asymmetry, resource.Cadence,
            resource.ImpactPeak);
}