using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Analytics.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClinicalReportRepository(AppDbContext context)
    : BaseRepository<ClinicalReport>(context), IClinicalReportRepository
{
    public new async Task<ClinicalReport?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public new async Task<IEnumerable<ClinicalReport>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .OrderByDescending(r => r.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClinicalReport?> FindByReportCodeAsync(ReportCode reportCode,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .FirstOrDefaultAsync(r => r.ReportCode == reportCode, cancellationToken);
    }

    public async Task<IEnumerable<ClinicalReport>> ListByPatientIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .Where(r => r.PatientId == patientId)
            .OrderByDescending(r => r.Id)
            .ToListAsync(cancellationToken);
    }
}