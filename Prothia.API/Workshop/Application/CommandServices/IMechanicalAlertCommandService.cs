using Prothia.API.Shared.Application.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;

namespace Prothia.API.Workshop.Application.CommandServices;

public interface IMechanicalAlertCommandService
{
    Task<Result<MechanicalAlert>> Handle(CreateMechanicalAlertCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<MechanicalAlert>> Handle(ResolveMechanicalAlertCommand command,
        CancellationToken cancellationToken = default);
}