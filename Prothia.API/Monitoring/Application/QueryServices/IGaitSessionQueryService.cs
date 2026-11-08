using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;

namespace Prothia.API.Monitoring.Application.QueryServices;

public interface IGaitSessionQueryService
{
    Task<IEnumerable<GaitSession>> Handle(GetAllGaitSessionsQuery query, CancellationToken cancellationToken = default);
    Task<GaitSession?> Handle(GetGaitSessionByIdQuery query, CancellationToken cancellationToken = default);
}