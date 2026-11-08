using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Application.Internal.OutboundServices;

/// <summary>
///     Outbound port through which Communication asks the Patients bounded context about patients.
/// </summary>
public interface IExternalPatientService
{
    Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default);

    Task<string?> FetchPatientFullNameByIdAsync(PatientId patientId, CancellationToken cancellationToken = default);
}