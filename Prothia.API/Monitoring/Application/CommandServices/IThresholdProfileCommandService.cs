using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;

namespace Prothia.API.Monitoring.Application.CommandServices;

public interface IThresholdProfileCommandService
{
    Task<Result<ThresholdProfile>> Handle(CreateThresholdProfileCommand command,
        CancellationToken cancellationToken = default);
    Task<Result<ThresholdProfile>> Handle(UpdateThresholdProfileCommand command,
        CancellationToken cancellationToken = default);
}
