using Microsoft.EntityFrameworkCore;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Monitoring.Infrastructure.Repositories;

public class ThresholdProfileRepository(AppDbContext context)
    : BaseRepository<ThresholdProfile>(context), IThresholdProfileRepository
{
    public async Task<IEnumerable<ThresholdProfile>> FindAllAsync(PatientId? patientId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<ThresholdProfile>().AsNoTracking();
        if (patientId is not null) query = query.Where(t => t.PatientId == patientId);
        return await query.OrderBy(t => t.PatientId).ToListAsync(cancellationToken);
    }

    public async Task<ThresholdProfile?> FindByPatientIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ThresholdProfile>()
            .FirstOrDefaultAsync(t => t.PatientId == patientId, cancellationToken);
    }
}
