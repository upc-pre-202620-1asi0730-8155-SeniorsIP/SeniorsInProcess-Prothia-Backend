using Prothia.API.Prescriptions.Application.Errors;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Prescriptions.Application.Services;

public interface IExerciseCommandService
{
    Task<Result<Exercise, ECreateExerciseError>> Handle(CreateExerciseCommand command,  
        CancellationToken cancellationToken = default);
}