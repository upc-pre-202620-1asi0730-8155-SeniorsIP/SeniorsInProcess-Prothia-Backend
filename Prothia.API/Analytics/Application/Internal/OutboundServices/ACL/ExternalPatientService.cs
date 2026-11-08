using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.ACL;

namespace Prothia.API.Analytics.Application.Internal.OutboundServices.ACL;

public class ExternalPatientService(IPatientsContextFacade patientsContextFacade)
{
    public Task<bool> ExistsPatientByIdAsync(PatientId patientId, CancellationToken cancellationToken = default) =>
        patientsContextFacade.ExistsPatientByIdAsync(patientId.Value, cancellationToken);

    public Task<string?> FetchPatientFullNameByIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default) =>
        patientsContextFacade.FetchPatientFullNameByIdAsync(patientId.Value, cancellationToken);
}