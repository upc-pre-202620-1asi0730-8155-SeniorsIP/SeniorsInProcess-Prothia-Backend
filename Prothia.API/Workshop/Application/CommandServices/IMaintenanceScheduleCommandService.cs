using Prothia.API.Shared.Application.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;

namespace Prothia.API.Workshop.Application.CommandServices;

public interface IMaintenanceScheduleCommandService
{
    Task<Result<MaintenanceSchedule>> Handle(CreateMaintenanceScheduleCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<MaintenanceSchedule>> CompleteAsync(int id, string? notes,
        CancellationToken cancellationToken = default);
}
