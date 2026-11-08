using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class MaintenanceScheduleResourceFromEntityAssembler
{
    public static MaintenanceScheduleResource ToResourceFromEntity(MaintenanceSchedule entity) =>
        new(entity.Id.Value,
            entity.SerialNumber.Value,
            entity.PatientName,
            entity.ScheduledDate,
            entity.InterventionType,
            entity.TechnicianName,
            entity.Status.ToCamelCaseString(),
            entity.Notes);
}