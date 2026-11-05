using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Queries;

namespace Prothia.API.Analytics.Application.QueryServices;

public interface IClinicalReportQueryService
{
    Task<IEnumerable<ClinicalReport>> Handle(GetAllClinicalReportsQuery query,
        CancellationToken cancellationToken = default);

    Task<ClinicalReport?> Handle(GetClinicalReportByIdQuery query, CancellationToken cancellationToken = default);
}