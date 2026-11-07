using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.QueryServices;

public class ThresholdProfileQueryService(IThresholdProfileRepository thresholdProfileRepository)
    : IThresholdProfileQueryService
{
    public async Task<IEnumerable<ThresholdProfile>> Handle(GetAllThresholdProfilesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await thresholdProfileRepository.FindAllAsync(query.PatientId, cancellationToken);
    }

    public async Task<ThresholdProfile?> Handle(GetThresholdProfileByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await thresholdProfileRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}