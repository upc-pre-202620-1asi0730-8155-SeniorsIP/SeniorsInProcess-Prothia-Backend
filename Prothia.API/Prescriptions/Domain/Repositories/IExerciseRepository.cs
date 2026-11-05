using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Prescriptions.Domain.Repositories;

public interface IExerciseRepository : IBaseRepository<Exercise>
{
    Task<Exercise?> FindByIdAsNoTrackingAsync(ExerciseId id, CancellationToken cancellationToken = default);
    Task<Exercise?> FindByNameAsync(ExerciseName name, CancellationToken cancellationToken = default);
}