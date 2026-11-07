using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Monitoring.Domain.Repositories;

public interface IGaitSessionRepository : IBaseRepository<GaitSession>
{
    Task<IEnumerable<GaitSession>> FindAllAsync(PatientId? patientId, CancellationToken cancellationToken = default);
}
