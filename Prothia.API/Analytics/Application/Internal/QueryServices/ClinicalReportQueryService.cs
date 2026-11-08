using Prothia.API.Analytics.Application.QueryServices;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Queries;
using Prothia.API.Analytics.Domain.Repositories;

namespace Prothia.API.Analytics.Application.Internal.QueryServices;

public class ClinicalReportQueryService(IClinicalReportRepository clinicalReportRepository)
    : IClinicalReportQueryService
{
    public async Task<IEnumerable<ClinicalReport>> Handle(GetAllClinicalReportsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.PatientId is not null)
            return await clinicalReportRepository.ListByPatientIdAsync(query.PatientId, cancellationToken);

        return await clinicalReportRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<ClinicalReport?> Handle(GetClinicalReportByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicalReportRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}