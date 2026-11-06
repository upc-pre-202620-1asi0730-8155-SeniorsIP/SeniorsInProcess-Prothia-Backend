using Prothia.API.Analytics.Application.CommandServices;
using Prothia.API.Analytics.Application.Internal.OutboundServices.ACL;
using Prothia.API.Analytics.Domain.Model;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Analytics.Domain.Model.Errors;
using Prothia.API.Analytics.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Analytics.Application.Internal.CommandServices;

public class ClinicalReportCommandService(
    IClinicalReportRepository clinicalReportRepository,
    ExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork)
    : IClinicalReportCommandService
{
    public async Task<Result<ClinicalReport>> Handle(
        CreateClinicalReportCommand command, CancellationToken cancellationToken = default)
    {
        var patientName =
            await externalPatientService.FetchPatientFullNameByIdAsync(command.PatientId, cancellationToken);
        if (patientName is null)
            return Result<ClinicalReport>.Failure(AnalyticsErrors.PatientNotFound, AnalyticsError.PatientNotFound);

        var existing =
            await clinicalReportRepository.FindByReportCodeAsync(command.ReportCode, cancellationToken);
        if (existing is not null)
            return Result<ClinicalReport>.Failure(AnalyticsErrors.DuplicateReportCode,
                AnalyticsError.DuplicateReportCode);

        var report = new ClinicalReport(command, patientName);
        await clinicalReportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ClinicalReport>.Success(report);
    }
}