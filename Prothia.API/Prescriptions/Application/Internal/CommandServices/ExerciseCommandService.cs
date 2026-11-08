using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Domain.Model;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.Errors;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.CommandServices;

public class ExerciseCommandService(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork)
    : IExerciseCommandService
{
    public async Task<Result<Exercise>> Handle(CreateExerciseCommand command,
        CancellationToken cancellationToken = default)
    {
        var existing = await exerciseRepository.FindByNameAsync(command.Name, cancellationToken);
        if (existing is not null)
            return Result<Exercise>.Failure(PrescriptionsErrors.DuplicateExerciseName,
                PrescriptionsError.DuplicateExerciseName);

        var exercise = new Exercise(command);
        await exerciseRepository.AddAsync(exercise, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Exercise>.Success(exercise);
    }
}