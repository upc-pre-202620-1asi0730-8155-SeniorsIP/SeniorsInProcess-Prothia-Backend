using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Analytics.Application.CommandServices;

public interface IClinicalReportCommandService
{
    Task<Result<ClinicalReport>> Handle(GenerateClinicalReportCommand command,
        CancellationToken cancellationToken = default);
}