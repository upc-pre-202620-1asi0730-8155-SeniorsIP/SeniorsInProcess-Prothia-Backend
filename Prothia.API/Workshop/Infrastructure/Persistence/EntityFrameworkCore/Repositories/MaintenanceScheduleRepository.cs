using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceScheduleRepository(AppDbContext context)
    : BaseRepository<MaintenanceSchedule>(context), IMaintenanceScheduleRepository
{
}