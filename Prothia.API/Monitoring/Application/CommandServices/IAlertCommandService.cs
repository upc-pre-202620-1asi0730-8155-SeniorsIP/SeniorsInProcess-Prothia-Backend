using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;

namespace Prothia.API.Monitoring.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default);
}
