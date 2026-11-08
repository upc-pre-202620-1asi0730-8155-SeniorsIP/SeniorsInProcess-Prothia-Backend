using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.CommandServices;

public class MaintenanceScheduleCommandService(
    IMaintenanceScheduleRepository maintenanceScheduleRepository,
    IUnitOfWork unitOfWork)
    : IMaintenanceScheduleCommandService
{
    public async Task<Result<MaintenanceSchedule>> Handle(
        CreateMaintenanceScheduleCommand command, CancellationToken cancellationToken = default)
    {
        var schedule = new MaintenanceSchedule(command);
        await maintenanceScheduleRepository.AddAsync(schedule, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MaintenanceSchedule>.Success(schedule);
    }
}