using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Monitoring.Application.CommandServices;

public interface IGaitSessionCommandService
{
    Task<Result<GaitSession>> Handle(RecordGaitSessionCommand command, CancellationToken cancellationToken = default);
}