using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Application.Internal.OutboundServices.ACL;
using Prothia.API.Prescriptions.Domain.Model;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.Errors;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.CommandServices;

public class PrescriptionCommandService(
    IPrescriptionRepository prescriptionRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IPrescriptionCommandService
{
    public async Task<Result<Prescription>> Handle(CreatePrescriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var patientExists =
            await externalPatientService.ExistsPatientByIdAsync(command.PatientId, cancellationToken);
        if (!patientExists)
            return Result<Prescription>.Failure(PrescriptionsErrors.PatientNotFound,
                PrescriptionsError.PatientNotFound);

        var prescription = new Prescription(command);
        await prescriptionRepository.AddAsync(prescription, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Prescription>.Success(prescription);
    }

    public async Task<Result<Prescription>> Handle(UpdatePrescriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var prescription = await prescriptionRepository.FindByIdAsync(command.PrescriptionId, cancellationToken);
        if (prescription is null)
            return Result<Prescription>.Failure(PrescriptionsErrors.PrescriptionNotFound,
                PrescriptionsError.PrescriptionNotFound);

        prescription.UpdatePlan(
            command.PhaseNumber,
            command.PhaseName,
            command.DurationWeeks,
            command.FrequencyPerWeek,
            command.Exercises);

        prescriptionRepository.Update(prescription);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Prescription>.Success(prescription);
    }
}
