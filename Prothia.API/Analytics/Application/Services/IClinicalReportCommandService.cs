using Prothia.API.Analytics.Application.Errors;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Analytics.Application.Services;

public interface IClinicalReportCommandService
{
    Task<Result<ClinicalReport, ECreateClinicalReportError>> Handle(CreateClinicalReportCommand command,
        CancellationToken cancellationToken = default);
}