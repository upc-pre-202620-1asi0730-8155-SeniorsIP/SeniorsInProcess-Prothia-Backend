using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.QueryServices;

public class MaintenanceScheduleQueryService(IMaintenanceScheduleRepository maintenanceScheduleRepository)
    : IMaintenanceScheduleQueryService
{
    public async Task<IEnumerable<MaintenanceSchedule>> Handle(GetAllMaintenanceSchedulesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await maintenanceScheduleRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<MaintenanceSchedule?> Handle(GetMaintenanceScheduleByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await maintenanceScheduleRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}