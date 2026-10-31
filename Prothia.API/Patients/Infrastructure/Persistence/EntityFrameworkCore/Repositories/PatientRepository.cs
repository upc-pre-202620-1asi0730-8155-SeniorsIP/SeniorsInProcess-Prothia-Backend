using Microsoft.EntityFrameworkCore;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Patients.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Patients.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PatientRepository(AppDbContext context) : BaseRepository<Patient>(context), IPatientRepository
{
    public async Task<Patient?> FindByDniAsync(Dni dni, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Patient>().FirstOrDefaultAsync(p => p.Dni == dni, cancellationToken);
    }
}