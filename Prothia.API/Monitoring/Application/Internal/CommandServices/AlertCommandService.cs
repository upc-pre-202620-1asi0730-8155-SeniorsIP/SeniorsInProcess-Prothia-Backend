using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Domain.Model;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.Errors;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.CommandServices;

public class AlertCommandService(
    IAlertRepository alertRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IAlertCommandService
{
    public async Task<Result<Alert>> Handle(CreateAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        var patientName =
            await externalPatientService.FetchPatientFullNameByIdAsync(command.PatientId, cancellationToken);
        if (patientName is null)
            return Result<Alert>.Failure(MonitoringErrors.PatientNotFound, MonitoringError.PatientNotFound);

        var alert = new Alert(command, patientName);
        await alertRepository.AddAsync(alert, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Alert>.Success(alert);
    }

    public async Task<Result<Alert>> Handle(ResolveAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(MonitoringErrors.AlertNotFound, MonitoringError.AlertNotFound);

        if (!alert.IsPending)
            return Result<Alert>.Failure(MonitoringErrors.AlertAlreadyResolved, MonitoringError.AlertAlreadyResolved);

        alert.Resolve(DateTimeOffset.UtcNow);
        alertRepository.Update(alert);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Alert>.Success(alert);
    }
}