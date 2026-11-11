using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;

namespace Prothia.API.Workshop.Application.QueryServices;

public interface IMechanicalAlertQueryService
{
    Task<IEnumerable<MechanicalAlerts>> Handle(GetAllMechanicalAlertsQuery query,
        CancellationToken cancellationToken = default);

    Task<MechanicalAlerts?> Handle(GetMechanicalAlertByIdQuery query, CancellationToken cancellationToken = default);
}