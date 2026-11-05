using Prothia.API.Patients.Application.Services;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Patients.Domain.Model.Errors;
using Prothia.API.Patients.Domain.Repositories;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Patients.Application.Internal.CommandServices;

public class PatientCommandService(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : IPatientCommandService
{
    public async Task<Result<Patient>> Handle(CreatePatientCommand command,
        CancellationToken cancellationToken = default)
    {
        if(await patientRepository.ExistsByDniAsync(command.Dni, cancellationToken))
            return Result<Patient>.Failure(PatientsErrors.DniAlreadyRegistered, PatientsErrors.DniAlreadyRegistered);

        var patient = new Patient(command);
        await patientRepository.AddAsync(patient, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Patient>.Success(patient);
}


        
        
