using Prothia.API.Patients.Application.QueryServices;
using Prothia.API.Patients.Domain.Model.Queries;

namespace Prothia.API.Patients.Interfaces.ACL;

public class PatientsContextFacade(IPatientQueryService patientQueryService) : IPatientsContextFacade
{
    public async Task<bool> ExistsPatientByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await patientQueryService.Handle(new GetPatientByIdQuery(patientId), cancellationToken);
        return patient is not null;
    }
    
    public async Task<string?> FetchPatientFullNameByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await patientQueryService.Handle(new GetPatientByIdQuery(patientId), cancellationToken);
        return patient?.FullName.Value;
    }
    
}