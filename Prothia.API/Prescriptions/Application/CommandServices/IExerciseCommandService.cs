using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Prescriptions.Application.CommandServices;

public interface IExerciseCommandService
{
    Task<Result<Exercise>> Handle(CreateExerciseCommand command, CancellationToken cancellationToken = default);
}