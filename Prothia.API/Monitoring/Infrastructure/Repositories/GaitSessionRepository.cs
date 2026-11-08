using Microsoft.EntityFrameworkCore;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Monitoring.Infrastructure.Repositories;

public class GaitSessionRepository(AppDbContext context)
    : BaseRepository<GaitSession>(context), IGaitSessionRepository
{
    public async Task<IEnumerable<GaitSession>> FindAllAsync(PatientId? patientId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<GaitSession>().AsNoTracking();
        if (patientId is not null) query = query.Where(s => s.PatientId == patientId);
        return await query.OrderByDescending(s => s.RecordedAt).ToListAsync(cancellationToken);
    }
}