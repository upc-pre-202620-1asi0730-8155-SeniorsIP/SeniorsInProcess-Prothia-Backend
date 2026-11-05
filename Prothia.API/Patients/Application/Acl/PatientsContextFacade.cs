using Prothia.API.Patients.Application.Services;
using Prothia.API.Patients.Domain.Model.Queries;
using Prothia.API.Patients.Interfaces.ACL;

namespace Prothia.API.Patients.Application.Acl;

public class PatientsContextFacade(IPatientQueryService patientQueryService) : IPatientsContextFacade
{
    public async Task<bool> ExistsPatientByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        if (patientId <= 0) return false;
        var patient = await patientQueryService.Handle(new GetPatientByIdQuery(patientId), cancellationToken);
        return patient is not null;
    }

    public async Task<string?> FetchPatientFullNameByIdAsync(int patientId,
        CancellationToken cancellationToken = default)
    {
        if (patientId <= 0) return null;
        var patient = await patientQueryService.Handle(new GetPatientByIdQuery(patientId), cancellationToken);
        return patient?.FullName.Value;
    }
}

