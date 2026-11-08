using Microsoft.EntityFrameworkCore;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClinicalTransmissionRepository(AppDbContext context)
    : BaseRepository<ClinicalTransmission>(context), IClinicalTransmissionRepository
{
    public async Task<IEnumerable<ClinicalTransmission>> ListByPatientIdAsync(PatientId? patientId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<ClinicalTransmission>().AsNoTracking();
        if (patientId is not null) query = query.Where(t => t.PatientId == patientId);
        return await query.OrderByDescending(t => t.Id).ToListAsync(cancellationToken);
    }
}