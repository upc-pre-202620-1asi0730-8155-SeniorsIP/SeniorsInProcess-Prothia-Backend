using Microsoft.EntityFrameworkCore;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Prescriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PrescriptionRepository(AppDbContext context)
    : BaseRepository<Prescription>(context), IPrescriptionRepository
{
    public override async Task<Prescription?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var prescriptionId = new PrescriptionId(id);
        return await Context.Set<Prescription>()
            .Include(p => p.Exercises)
            .FirstOrDefaultAsync(p => p.Id == prescriptionId, cancellationToken);
    }

    public override async Task<Prescription?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        return await FindByIdAsNoTrackingAsync(new PrescriptionId(id), cancellationToken);
    }

    public async Task<Prescription?> FindByIdAsNoTrackingAsync(PrescriptionId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Prescription>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Exercises)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Prescription>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Prescription>()
            .Include(p => p.Exercises)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Prescription>> ListAsNoTrackingAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Prescription>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Exercises)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Prescription>> ListByPatientIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Prescription>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Exercises)
            .Where(p => p.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
