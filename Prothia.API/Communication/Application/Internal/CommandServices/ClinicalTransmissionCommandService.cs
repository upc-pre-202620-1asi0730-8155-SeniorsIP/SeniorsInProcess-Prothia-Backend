using Prothia.API.Communication.Application.CommandServices;
using Prothia.API.Communication.Application.Internal.OutboundServices.ACL;
using Prothia.API.Communication.Domain.Model;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.Errors;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling clinical transmission commands.
/// </summary>
public class ClinicalTransmissionCommandService(
    IClinicalTransmissionRepository clinicalTransmissionRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IClinicalTransmissionCommandService
{
    /// <inheritdoc />
    public async Task<Result<ClinicalTransmission>> Handle(
        CreateClinicalTransmissionCommand command, CancellationToken cancellationToken = default)
    {
        var patientName =
            await externalPatientService.FetchPatientFullNameByIdAsync(command.PatientId, cancellationToken);
        if (patientName is null)
            return Result<ClinicalTransmission>.Failure(CommunicationErrors.PatientNotFound,
                CommunicationError.PatientNotFound);

        var transmission = new ClinicalTransmission(command, patientName);
        await clinicalTransmissionRepository.AddAsync(transmission, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ClinicalTransmission>.Success(transmission);
    }
}