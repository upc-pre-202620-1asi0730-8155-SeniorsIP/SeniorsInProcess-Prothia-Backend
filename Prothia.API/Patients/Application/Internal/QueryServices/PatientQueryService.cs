using Prothia.API.Patients.Application.Services;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Queries;
using Prothia.API.Patients.Domain.Repositories;

namespace Prothia.API.Patients.Application.Internal.QueryServices;

public class PatientQueryService(IPatientRepository patientRepository) : IPatientQueryService
{
    public async Task<IEnumerable<Patient>> Handle(GetAllPatientsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await patientRepository.ListAsync(cancellationToken);
    }
    
    public async Task<Patient?> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await patientRepository.FindByIdAsync(query.Id, cancellationToken);
    }
    
    public async Task<Patient?> Handle(GetPatientByDniQuery query, CancellationToken cancellationToken = default)
    {
        return await patientRepository.FindByDniAsync(query.Dni, cancellationToken);
    }
}