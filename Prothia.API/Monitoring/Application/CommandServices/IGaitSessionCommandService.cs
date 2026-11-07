using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;

namespace Prothia.API.Monitoring.Application.CommandServices;

public interface IGaitSessionCommandService
{
    Task<Result<GaitSession>> Handle(RecordGaitSessionCommand command, CancellationToken cancellationToken = default);
}