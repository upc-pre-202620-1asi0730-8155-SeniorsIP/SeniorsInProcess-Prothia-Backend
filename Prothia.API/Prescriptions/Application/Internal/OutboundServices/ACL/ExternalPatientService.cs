using Prothia.API.Patients.Interfaces.ACL;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Application.Internal.OutboundServices.ACL;

/// <summary>
///     Anti-corruption layer adapter: translates Prescriptions' needs into calls to the Patients context facade.
/// </summary>
public class ExternalPatientService(IPatientsContextFacade patientsContextFacade)
{
    public Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default) =>
        patientsContextFacade.ExistsPatientByIdAsync(patientId.Value, cancellationToken);
}