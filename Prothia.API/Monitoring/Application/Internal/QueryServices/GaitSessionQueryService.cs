using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.QueryServices;

public class GaitSessionQueryService(IGaitSessionRepository gaitSessionRepository) : IGaitSessionQueryService
{
    public async Task<IEnumerable<GaitSession>> Handle(GetAllGaitSessionsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await gaitSessionRepository.FindAllAsync(query.PatientId, cancellationToken);
    }

    public async Task<GaitSession?> Handle(GetGaitSessionByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await gaitSessionRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}