using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class CreateProsthesisCommandFromResourceAssembler
{
    public static CreateProsthesisCommand ToCommandFromResource(CreateProsthesisResource resource) =>
        new(new SerialNumber(resource.SerialNumber),
            resource.Type,
            resource.PatientName ?? string.Empty,
            resource.ClinicName ?? string.Empty,
            resource.AccumulatedCycles,
            resource.CycleLimit,
            resource.LastService ?? string.Empty,
            EnumConversions.Parse<ProsthesisStatus>(resource.Status, nameof(resource.Status)),
            resource.BatteryLevel,
            EnumConversions.Parse<SensorStatus>(resource.SensorStatus, nameof(resource.SensorStatus)),
            EnumConversions.Parse<WorkshopKLevel>(resource.KLevel, nameof(resource.KLevel)),
            resource.SocketType ?? string.Empty,
            resource.KneeType ?? string.Empty,
            resource.FootType ?? string.Empty);
}