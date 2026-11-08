using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Domain.Repositories;

public interface IClinicalTransmissionRepository : IBaseRepository<ClinicalTransmission>
{
    Task<IEnumerable<ClinicalTransmission>> ListByPatientIdAsync(PatientId? patientId,
        CancellationToken cancellationToken = default);
}