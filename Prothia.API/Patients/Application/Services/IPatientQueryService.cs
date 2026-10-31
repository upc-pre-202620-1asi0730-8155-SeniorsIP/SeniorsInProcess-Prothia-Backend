using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Queries;

namespace Prothia.API.Patients.Application.Services;

public interface IPatientQueryService
{
    Task<IEnumerable<Patient>> Handle(GetAllPatientsQuery query, CancellationToken cancellationToken = default);

    Task<Patient?> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken = default);

    Task<Patient?> Handle(GetPatientByDniQuery query, CancellationToken cancellationToken = default);
}