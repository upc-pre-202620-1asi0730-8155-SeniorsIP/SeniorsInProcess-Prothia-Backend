using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.ACL;

namespace Prothia.API.Communication.Application.Internal.OutboundServices.ACL;

/// <summary>
///     Anti-corruption layer adapter: translates Communication's needs into calls to the Patients context facade.
/// </summary>
public class ExternalPatientService(IPatientsContextFacade patientsContextFacade)
{
    public Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default) =>
        patientsContextFacade.ExistsPatientByIdAsync(patientId.Value, cancellationToken);

    public Task<string?> FetchPatientFullNameByIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default) =>
        patientsContextFacade.FetchPatientFullNameByIdAsync(patientId.Value, cancellationToken);
}