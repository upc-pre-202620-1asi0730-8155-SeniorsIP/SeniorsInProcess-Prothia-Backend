using Prothia.API.Monitoring.Application.Internal.Acl;
using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Domain.Model;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.Errors;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Monitoring.Application.Internal.CommandServices;

public class GaitSessionCommandService(
    IGaitSessionRepository gaitSessionRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IGaitSessionCommandService
{
    public async Task<Result<GaitSession>> Handle(RecordGaitSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var patientExists =
            await externalPatientService.ExistsPatientByIdAsync(command.PatientId, cancellationToken);
        if (!patientExists)
            return Result<GaitSession>.Failure(MonitoringErrors.PatientNotFound, MonitoringError.PatientNotFound);

        var session = new GaitSession(command);
        await gaitSessionRepository.AddAsync(session, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<GaitSession>.Success(session);
    }
}