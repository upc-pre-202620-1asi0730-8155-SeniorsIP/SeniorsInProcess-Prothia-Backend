using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity) =>
        new(entity.Id.Value,
            entity.PatientId.Value,
            entity.PatientName,
            entity.DeviationType.Value,
            entity.RecordedValue.Value,
            entity.Threshold.Value,
            entity.Severity.ToString().ToLowerInvariant(),
            entity.Status.ToString().ToLowerInvariant(),
            entity.OccurredAt,
            entity.ResolvedAt);
}