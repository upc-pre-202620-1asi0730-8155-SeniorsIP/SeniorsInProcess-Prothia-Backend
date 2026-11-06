using Microsoft.EntityFrameworkCore;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Prescriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ExerciseRepository(AppDbContext context) : BaseRepository<Exercise>(context), IExerciseRepository
{
    public async Task<Exercise?> FindByIdAsNoTrackingAsync(ExerciseId id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Exercise>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Exercise?> FindByNameAsync(ExerciseName name, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Exercise>().FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
    }
}