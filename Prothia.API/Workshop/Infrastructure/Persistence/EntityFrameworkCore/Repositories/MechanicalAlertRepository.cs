using Microsoft.EntityFrameworkCore;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MechanicalAlertRepository(AppDbContext context)
    : BaseRepository<MechanicalAlerts>(context), IMechanicalAlertRepository
{
    public async Task<IEnumerable<MechanicalAlerts>> FindAllAsync(int? prosthesisId, MechanicalAlertStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<MechanicalAlerts>().AsNoTracking();
        if (prosthesisId is not null)
        {
            var prosthesisIdValue = prosthesisId.Value;
            query = query.Where(a => a.ProsthesisId == prosthesisIdValue);
        }

        if (status is not null)
        {
            var statusValue = status.Value;
            query = query.Where(a => a.Status == statusValue);
        }

        return await query.OrderByDescending(a => a.Id).ToListAsync(cancellationToken);
    }
}