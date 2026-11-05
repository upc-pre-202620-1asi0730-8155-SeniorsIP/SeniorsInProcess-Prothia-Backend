using Prothia.API.Analytics.Application.Errors;
using Prothia.API.Analytics.Application.Internal.OutboundServices;
using Prothia.API.Analytics.Application.Services;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Analytics.Domain.Repositories;
using Prothia.API.Shared.Application.Extensions;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Analytics.Application.Internal.CommandServices;


public class ClinicalReportCommandService(
    IClinicalReportRepository clinicalReportRepository,
    IExternalPatientService externalPatientService,
    IUnitOfWork unitOfWork,
    ILogger<ClinicalReportCommandService> logger)
    : IClinicalReportCommandService
{
    public async Task<Result<ClinicalReport, ECreateClinicalReportError>> Handle(
        CreateClinicalReportCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var patientName =
                await externalPatientService.FetchPatientFullNameByIdAsync(command.PatientId, cancellationToken);
            if (patientName is null)
            {
                logger.LogWarning("Clinical report rejected: patient {PatientId} does not exist", command.PatientId);
                return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                    ECreateClinicalReportError.PatientNotFound);
            }

            var existing =
                await clinicalReportRepository.FindByReportCodeAsync(command.ReportCode, cancellationToken);
            if (existing is not null)
            {
                logger.LogWarning("Duplicate clinical report rejected for report code {ReportCode}",
                    command.ReportCode);
                return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                    ECreateClinicalReportError.DuplicateReportCode);
            }

            var report = new ClinicalReport(command, patientName);
            await clinicalReportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<ClinicalReport, ECreateClinicalReportError>.Success(report);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating clinical report {ReportCode}", command.ReportCode);
            return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                ECreateClinicalReportError.InvalidClinicalReport);
        }
        catch (DbUpdateException ex) when (ex.IsDuplicateKeyViolation())
        {
            logger.LogWarning(ex, "Duplicate key violation creating clinical report {ReportCode}", command.ReportCode);
            return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                ECreateClinicalReportError.DuplicateReportCode);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating clinical report {ReportCode}", command.ReportCode);
            return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                ECreateClinicalReportError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating clinical report {ReportCode}", command.ReportCode);
            return new Result<ClinicalReport, ECreateClinicalReportError>.Failure(
                ECreateClinicalReportError.UnexpectedError);
        }
    }
}
