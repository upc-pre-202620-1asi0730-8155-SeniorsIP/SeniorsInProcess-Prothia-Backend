using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ProsthesisResource from a Prosthesis.
/// </summary>
public static class ProsthesisResourceFromEntityAssembler
{
    public static ProsthesisResource ToResourceFromEntity(Prosthesis entity) =>
        new(entity.Id.Value,
            entity.SerialNumber.Value,
            entity.Type,
            entity.PatientName,
            entity.ClinicName,
            entity.AccumulatedCycles,
            entity.CycleLimit,
            entity.LastService,
            entity.Status.ToString().ToLowerInvariant(),
            entity.BatteryLevel,
            entity.SensorStatus.ToString().ToLowerInvariant(),
            entity.KLevel.ToString(),
            entity.SocketType,
            entity.KneeType,
            entity.FootType);
}