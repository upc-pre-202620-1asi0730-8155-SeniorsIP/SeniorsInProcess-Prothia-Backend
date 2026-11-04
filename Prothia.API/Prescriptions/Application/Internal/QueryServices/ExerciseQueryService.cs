using Prothia.API.Prescriptions.Application.Services;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Queries;
using Prothia.API.Prescriptions.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.QueryServices;

public class ExerciseQueryService(IExerciseRepository exerciseRepository): IExerciseQueryService
{
    public async Task<IEnumerable<Exercise>> Handle(GetAllExercisesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await exerciseRepository.ListAsync(cancellationToken);
    }

    public async Task<Exercise?> Handle(GetExerciseByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await exerciseRepository.FindByIdAsync(query.Id, cancellationToken);

    }
}