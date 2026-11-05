using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Analytics.Domain.Repositories;

public interface IClinicalReportRepository : IBaseRepository<ClinicalReport>
{
    Task<ClinicalReport?> FindByReportCodeAsync(ReportCode reportCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClinicalReport>> ListByPatientIdAsync(PatientId patientId,
        CancellationToken cancellationToken = default);
}