using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public static class ThresholdProfileResourceFromEntityAssembler
{
    public static ThresholdProfileResource ToResourceFromEntity(ThresholdProfile entity) =>
        new(entity.Id.Value, entity.PatientId.Value, entity.PelvicTilt, entity.Asymmetry, entity.Cadence,
            entity.ImpactPeak);
}