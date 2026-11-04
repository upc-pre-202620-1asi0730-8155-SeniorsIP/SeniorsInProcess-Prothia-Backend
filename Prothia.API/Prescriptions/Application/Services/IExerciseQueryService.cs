using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Queries;

namespace Prothia.API.Prescriptions.Application.Services;

public interface IExerciseQueryService
{
    Task<IEnumerable<Exercise>> Handle(GetAllExercisesQuery query ,CancellationToken cancellationToken = default);
    Task<Exercise?> Handle(GetExerciseByIdQuery query, CancellationToken cancellationToken = default);
}