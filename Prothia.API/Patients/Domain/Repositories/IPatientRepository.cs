using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Patients.Domain.Repositories;

public interface IPatientRepository : IBaseRepository<Patient>
{
    Task<Patient?> FindByDniAsync(Dni dni, CancellationToken cancellationToken = default);
    Task<Patient?> FindByDniAsNoTrackingAsync(Dni dni, CancellationToken cancellationToken = default);
    Task<Patient?> FindByIdAsNoTrackingAsync(PatientId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDniAsync(Dni dni, CancellationToken cancellationToken = default);
}