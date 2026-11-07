using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await alertRepository.FindAllAsync(query.PatientId, query.Status, cancellationToken);
    }

    public async Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await alertRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}