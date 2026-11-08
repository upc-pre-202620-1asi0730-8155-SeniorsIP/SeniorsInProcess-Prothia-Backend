using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class CreateMaintenanceScheduleCommandFromResourceAssembler
{
    public static CreateMaintenanceScheduleCommand ToCommandFromResource(CreateMaintenanceScheduleResource resource) =>
        new(new SerialNumber(resource.SerialNumber),
            resource.PatientName,
            resource.ScheduledDate,
            resource.InterventionType,
            resource.TechnicianName,
            EnumConversions.Parse<MaintenanceStatus>(resource.Status, nameof(resource.Status)),
            resource.Notes ?? string.Empty);
}