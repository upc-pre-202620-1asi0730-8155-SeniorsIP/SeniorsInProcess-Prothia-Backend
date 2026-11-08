using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Monitoring.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default);
}
