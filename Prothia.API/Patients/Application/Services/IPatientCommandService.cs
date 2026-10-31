using Prothia.API.Patients.Application.Errors;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Patients.Application.Services;

public interface IPatientCommandService
{
    Task<Result<Patient, ECreatePatientError>> Handle(
        CreatePatientCommand command,
        CancellationToken cancellationToken = default);
}