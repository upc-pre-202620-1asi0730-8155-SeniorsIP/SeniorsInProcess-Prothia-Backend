using Prothia.API.Shared.Application.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;

namespace Prothia.API.Workshop.Application.CommandServices;

public interface IMechanicalAlertCommandService
{
    Task<Result<MechanicalAlerts>> Handle(CreateMechanicalAlertCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<MechanicalAlerts>> Handle(SolveMechanicalAlertCommand command,
        CancellationToken cancellationToken = default);
}