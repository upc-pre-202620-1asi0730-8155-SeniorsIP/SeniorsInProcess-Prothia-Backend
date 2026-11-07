using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public static class GaitSessionResourceFromEntityAssembler
{
    public static GaitSessionResource ToResourceFromEntity(GaitSession entity) =>
        new(entity.Id.Value,
            entity.PatientId.Value,
            entity.RecordedAt,
            entity.DurationMinutes,
            entity.Steps,
            entity.Cadence,
            entity.SymmetryPercent,
            entity.SymmetryGrade.ToCamelCaseString(),
            entity.AlertsCount,
            entity.ImpactPeakNewtons);
}