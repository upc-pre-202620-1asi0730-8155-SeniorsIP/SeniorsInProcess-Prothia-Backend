using Microsoft.EntityFrameworkCore;
using Prothia.API.Patients.Application.Errors;
using Prothia.API.Patients.Application.Services;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Patients.Domain.Repositories;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Patients.Application.Internal.CommandServices;

public class PatientCommandService(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork,
    ILogger<PatientCommandService> logger)
    : IPatientCommandService
{
    public async Task<Result<Patient, ECreatePatientError>> Handle(CreatePatientCommand command,
        CancellationToken cancellationToken = default)
    {
        var existingPatient = await patientRepository.FindByDniAsync(command.Dni, cancellationToken);
        if (existingPatient != null)
        {
            logger.LogWarning("Duplicate patient rejected for Dni {Dni}", command.Dni.MaskedForLogging());
            return new Result<Patient, ECreatePatientError>.Failure(ECreatePatientError.DuplicatePatient);
        }

        try
        {
            var patient = new Patient(command);
            await patientRepository.AddAsync(patient, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Patient, ECreatePatientError>.Success(patient);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating patient for Dni {Dni}",
                command.Dni.MaskedForLogging());
            return new Result<Patient, ECreatePatientError>.Failure(ECreatePatientError.InvalidPatient);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating patient for Dni {Dni}",
                command.Dni.MaskedForLogging());
            return new Result<Patient, ECreatePatientError>.Failure(ECreatePatientError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating patient for Dni {Dni}", command.Dni.MaskedForLogging());
            return new Result<Patient, ECreatePatientError>.Failure(ECreatePatientError.UnexpectedError);
        }
    }
}


        
        
