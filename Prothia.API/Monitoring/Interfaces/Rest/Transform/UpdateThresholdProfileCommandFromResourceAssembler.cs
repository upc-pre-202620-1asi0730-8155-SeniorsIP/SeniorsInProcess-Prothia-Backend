using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public class UpdateThresholdProfileCommandFromResourceAssembler
{
    public static UpdateThresholdProfileCommand ToCommandFromResource(int thresholdProfileId,
        UpdateThresholdProfileResource resource) =>
        new(thresholdProfileId, resource.PelvicTilt, resource.Asymmetry, resource.Cadence, resource.ImpactPeak);
}