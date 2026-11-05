using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Patients.Application.CommandServices;

public interface IPatientCommandService
{
    Task<Result<Patient>>Handle(CreatePatientCommand command, CancellationToken cancellationToken = default);
}