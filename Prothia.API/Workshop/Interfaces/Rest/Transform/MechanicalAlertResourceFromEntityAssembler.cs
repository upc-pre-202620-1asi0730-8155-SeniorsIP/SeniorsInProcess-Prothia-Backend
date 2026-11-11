using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class MechanicalAlertResourceFromEntityAssembler
{
    public static MechanicalAlertResource ToResourceFromEntity(MechanicalAlerts entity) =>
        new(entity.Id.Value,
            entity.ProsthesisId,
            entity.SerialNumber.Value,
            entity.Cycles,
            entity.CycleLimit,
            entity.PatientName,
            entity.KLevel.ToString(),
            entity.Component,
            entity.Description,
            entity.Severity.ToString().ToLowerInvariant(),
            entity.Status.ToString().ToLowerInvariant(),
            entity.OccurredAt);
}