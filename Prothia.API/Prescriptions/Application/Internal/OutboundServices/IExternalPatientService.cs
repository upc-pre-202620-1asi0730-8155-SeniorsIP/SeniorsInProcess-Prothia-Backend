using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Application.Internal.OutboundServices;

/// <summary>
///     Outbound port through which Prescriptions asks the Patients bounded context about patients.
/// </summary>
public interface IExternalPatientService
{
    Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default);
}