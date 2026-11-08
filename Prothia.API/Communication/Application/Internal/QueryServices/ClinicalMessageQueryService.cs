using Prothia.API.Communication.Application.QueryServices;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.QueryServices;

public class ClinicalMessageQueryService(IClinicalMessageRepository clinicalMessageRepository)
    : IClinicalMessageQueryService
{
    public async Task<IEnumerable<ClinicalMessage>> Handle(GetAllClinicalMessagesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicalMessageRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<ClinicalMessage?> Handle(GetClinicalMessageByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicalMessageRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}