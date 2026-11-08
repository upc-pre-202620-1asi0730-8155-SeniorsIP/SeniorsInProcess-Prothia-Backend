using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;

namespace Prothia.API.Workshop.Application.QueryServices;

public interface IMechanicalAlertQueryService
{
    Task<IEnumerable<MechanicalAlert>> Handle(GetAllMechanicalAlertsQuery query,
        CancellationToken cancellationToken = default);

    Task<MechanicalAlert?> Handle(GetMechanicalAlertByIdQuery query, CancellationToken cancellationToken = default);
}