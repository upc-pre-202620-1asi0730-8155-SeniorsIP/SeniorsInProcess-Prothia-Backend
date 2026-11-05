using Microsoft.EntityFrameworkCore;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Analytics.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClinicalReportRepository(AppDbContext context)
    : BaseRepository<ClinicalReport>(context), IClinicalReportRepository
{
    public override async Task<ClinicalReport?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var reportId = new ClinicalReportId(id);
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }

    public override async Task<ClinicalReport?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var reportId = new ClinicalReportId(id);
        return await Context.Set<ClinicalReport>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Indicators)
            .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);
    }

    public override async Task<IEnumerable<ClinicalReport>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .Include(r => r.Indicators)
            .OrderByDescending(r => r.Id)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<ClinicalReport>> ListAsNoTrackingAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClinicalReport>()
            .AsNoTracking()
            .AsSplitQuery()
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

    public async Task<IEnumerable<ClinicalReport>> ListByPatientIdAsync(PatientId? patientId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<ClinicalReport>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Indicators)
            .AsQueryable();
        if (patientId is not null) query = query.Where(r => r.PatientId == patientId.Value);
        return await query.OrderByDescending(r => r.Id).ToListAsync(cancellationToken);
    }
}
