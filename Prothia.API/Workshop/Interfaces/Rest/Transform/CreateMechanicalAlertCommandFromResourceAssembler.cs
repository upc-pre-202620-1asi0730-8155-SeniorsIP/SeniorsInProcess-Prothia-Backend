using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class CreateMechanicalAlertCommandFromResourceAssembler
{
    public static CreateMechanicalAlertCommand ToCommandFromResource(CreateMechanicalAlertResource resource) =>
        new(resource.ProsthesisId,
            new SerialNumber(resource.SerialNumber),
            resource.Cycles,
            resource.CycleLimit,
            resource.PatientName ?? string.Empty,
            EnumConversions.Parse<WorkshopKLevel>(resource.KLevel, nameof(resource.KLevel)),
            resource.Component,
            resource.Description,
            EnumConversions.Parse<MechanicalAlertSeverity>(resource.Severity, nameof(resource.Severity)),
            string.IsNullOrWhiteSpace(resource.OccurredAt)
                ? DateTimeOffset.UtcNow.ToString("O")
                : resource.OccurredAt.Trim());
}