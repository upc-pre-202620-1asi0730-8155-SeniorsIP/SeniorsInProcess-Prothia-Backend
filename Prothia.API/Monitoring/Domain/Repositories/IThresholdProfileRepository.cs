using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Monitoring.Domain.Repositories;

public interface IThresholdProfileRepository : IBaseRepository<ThresholdProfile>
{
    Task<IEnumerable<ThresholdProfile>> FindAllAsync(PatientId? patientId,
        CancellationToken cancellationToken = default);
    Task<ThresholdProfile?> FindByPatientIdAsync(PatientId patientId, CancellationToken cancellationToken = default);
}