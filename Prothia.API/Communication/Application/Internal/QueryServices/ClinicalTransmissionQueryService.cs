using Prothia.API.Communication.Application.QueryServices;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.QueryServices;

public class ClinicalTransmissionQueryService(IClinicalTransmissionRepository clinicalTransmissionRepository)
    : IClinicalTransmissionQueryService
{
    public async Task<IEnumerable<ClinicalTransmission>> Handle(GetAllClinicalTransmissionsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicalTransmissionRepository.ListByPatientIdAsync(query.PatientId, cancellationToken);
    }

    public async Task<ClinicalTransmission?> Handle(GetClinicalTransmissionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicalTransmissionRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}