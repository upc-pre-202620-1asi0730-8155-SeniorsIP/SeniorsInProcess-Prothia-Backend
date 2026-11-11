using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Repositories;

public interface IMechanicalAlertRepository : IBaseRepository<MechanicalAlerts>
{
    Task<IEnumerable<MechanicalAlerts>> FindAllAsync(int? prosthesisId, MechanicalAlertStatus? status,
        CancellationToken cancellationToken = default);
}