using Microsoft.EntityFrameworkCore;
using Prothia.API.Prescriptions.Application.Errors;
using Prothia.API.Prescriptions.Application.Internal.OutboundServices;
using Prothia.API.Prescriptions.Application.Services;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.CommandServices;
/// <summary>
///     Application service for handling rehabilitation prescription commands.
/// </summary>
/// <param name="prescriptionRepository">Repository for accessing prescription data.</param>
/// <param name="externalPatientService">Outbound port to the Patients bounded context.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class PrescriptionCommandService (
    IPrescriptionRepository prescriptionRepository,
    IExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork,
    ILogger<PrescriptionCommandService> logger)
    : IPrescriptionCommandService
{
     public async Task<Result<Prescription, ECreatePrescriptionError>> Handle(CreatePrescriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!await externalPatientService.ExistsPatientByIdAsync(command.PatientId, cancellationToken))
            {
                logger.LogWarning("Prescription rejected: patient {PatientId} does not exist", command.PatientId);
                return new Result<Prescription, ECreatePrescriptionError>.Failure(
                    ECreatePrescriptionError.PatientNotFound);
            }

            var prescription = new Prescription(command);
            await prescriptionRepository.AddAsync(prescription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Prescription, ECreatePrescriptionError>.Success(prescription);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating prescription for patient {PatientId}",
                command.PatientId);
            return new Result<Prescription, ECreatePrescriptionError>.Failure(
                ECreatePrescriptionError.InvalidPrescription);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating prescription for patient {PatientId}",
                command.PatientId);
            return new Result<Prescription, ECreatePrescriptionError>.Failure(
                ECreatePrescriptionError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating prescription for patient {PatientId}", command.PatientId);
            return new Result<Prescription, ECreatePrescriptionError>.Failure(
                ECreatePrescriptionError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Prescription, EUpdatePrescriptionError>> Handle(UpdatePrescriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var prescription = await prescriptionRepository.FindByIdAsync(command.PrescriptionId, cancellationToken);
            if (prescription is null)
                return new Result<Prescription, EUpdatePrescriptionError>.Failure(
                    EUpdatePrescriptionError.PrescriptionNotFound);

            prescription.Update(command);
            prescriptionRepository.Update(prescription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Prescription, EUpdatePrescriptionError>.Success(prescription);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating prescription {PrescriptionId}",
                command.PrescriptionId);
            return new Result<Prescription, EUpdatePrescriptionError>.Failure(
                EUpdatePrescriptionError.InvalidPrescription);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating prescription {PrescriptionId}",
                command.PrescriptionId);
            return new Result<Prescription, EUpdatePrescriptionError>.Failure(
                EUpdatePrescriptionError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating prescription {PrescriptionId}", command.PrescriptionId);
            return new Result<Prescription, EUpdatePrescriptionError>.Failure(
                EUpdatePrescriptionError.UnexpectedError);
        }
    }
}