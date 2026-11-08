using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.QueryServices;

public class MechanicalAlertQueryService(IMechanicalAlertRepository mechanicalAlertRepository)
    : IMechanicalAlertQueryService
{
    public async Task<IEnumerable<MechanicalAlert>> Handle(GetAllMechanicalAlertsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await mechanicalAlertRepository.FindAllAsync(query.ProsthesisId, query.Status, cancellationToken);
    }

    public async Task<MechanicalAlert?> Handle(GetMechanicalAlertByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await mechanicalAlertRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}