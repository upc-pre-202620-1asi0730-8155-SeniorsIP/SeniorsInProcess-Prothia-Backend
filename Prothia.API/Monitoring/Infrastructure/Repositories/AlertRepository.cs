using Microsoft.EntityFrameworkCore;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Monitoring.Infrastructure.Repositories;

public class AlertRepository(AppDbContext context) : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> FindAllAsync(PatientId? patientId, EAlertStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Alert>().AsNoTracking();
        if (patientId is not null) query = query.Where(a => a.PatientId == patientId);
        if (status is not null)
        {
            var statusValue = status.Value;
            query = query.Where(a => a.Status == statusValue);
        }

        return await query.OrderByDescending(a => a.OccurredAt).ToListAsync(cancellationToken);
    }
}