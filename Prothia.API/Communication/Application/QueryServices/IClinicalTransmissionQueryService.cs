using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;

namespace Prothia.API.Communication.Application.QueryServices;

public interface IClinicalTransmissionQueryService
{
    Task<IEnumerable<ClinicalTransmission>> Handle(GetAllClinicalTransmissionsQuery query,
        CancellationToken cancellationToken = default);

    Task<ClinicalTransmission?> Handle(GetClinicalTransmissionByIdQuery query,
        CancellationToken cancellationToken = default);
}