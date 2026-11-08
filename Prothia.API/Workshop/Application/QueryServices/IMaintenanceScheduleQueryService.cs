using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;

namespace Prothia.API.Workshop.Application.QueryServices;

public interface IMaintenanceScheduleQueryService
{
    Task<IEnumerable<MaintenanceSchedule>> Handle(GetAllMaintenanceSchedulesQuery query,
        CancellationToken cancellationToken = default);

    Task<MaintenanceSchedule?> Handle(GetMaintenanceScheduleByIdQuery query,
        CancellationToken cancellationToken = default);
}