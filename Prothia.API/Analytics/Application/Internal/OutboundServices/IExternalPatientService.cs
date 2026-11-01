using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Application.Internal.OutboundServices;

public interface IExternalPatientService
{
    Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default);

    Task<string?> FetchPatientFullNameByIdAsync(PatientId patientId, CancellationToken cancellationToken = default);
    
}