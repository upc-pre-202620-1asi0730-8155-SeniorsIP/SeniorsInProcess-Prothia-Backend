using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Domain.Model;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.Errors;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.CommandServices;

public class ThresholdProfileCommandService(
    IThresholdProfileRepository thresholdProfileRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IThresholdProfileCommandService
{
    public async Task<Result<ThresholdProfile>> Handle(CreateThresholdProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var patientExists =
            await externalPatientService.ExistsPatientByIdAsync(command.PatientId, cancellationToken);
        if (!patientExists)
            return Result<ThresholdProfile>.Failure(MonitoringErrors.PatientNotFound, MonitoringError.PatientNotFound);

        var existing = await thresholdProfileRepository.FindByPatientIdAsync(command.PatientId, cancellationToken);
        if (existing is not null)
            return Result<ThresholdProfile>.Failure(MonitoringErrors.DuplicateThresholdProfileForPatient,
                MonitoringError.DuplicateThresholdProfileForPatient);

        var profile = new ThresholdProfile(command);
        await thresholdProfileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ThresholdProfile>.Success(profile);
    }

    public async Task<Result<ThresholdProfile>> Handle(UpdateThresholdProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var profile = await thresholdProfileRepository.FindByIdAsync(command.ThresholdProfileId, cancellationToken);
        if (profile is null)
            return Result<ThresholdProfile>.Failure(MonitoringErrors.ThresholdProfileNotFound,
                MonitoringError.ThresholdProfileNotFound);

        profile.Update(command);
        thresholdProfileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ThresholdProfile>.Success(profile);
    }
}