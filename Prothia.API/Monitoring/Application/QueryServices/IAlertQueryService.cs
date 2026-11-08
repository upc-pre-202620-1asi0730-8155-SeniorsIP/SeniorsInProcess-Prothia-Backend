using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Queries;

namespace Prothia.API.Monitoring.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query, CancellationToken cancellationToken = default);
    Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);
}