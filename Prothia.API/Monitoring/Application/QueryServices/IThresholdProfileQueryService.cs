using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;

namespace Prothia.API.Monitoring.Application.QueryServices;

public interface IThresholdProfileQueryService

{
    Task<IEnumerable<ThresholdProfile>> Handle(GetAllThresholdProfilesQuery query,
        CancellationToken cancellationToken = default);
    Task<ThresholdProfile?> Handle(GetThresholdProfileByIdQuery query, CancellationToken cancellationToken = default);
}