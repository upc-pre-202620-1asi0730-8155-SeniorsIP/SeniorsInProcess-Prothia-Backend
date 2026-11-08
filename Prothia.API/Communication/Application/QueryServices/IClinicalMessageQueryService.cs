using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;

namespace Prothia.API.Communication.Application.QueryServices;

public interface IClinicalMessageQueryService
{
    Task<IEnumerable<ClinicalMessage>> Handle(GetAllClinicalMessagesQuery query,
        CancellationToken cancellationToken = default);

    Task<ClinicalMessage?> Handle(GetClinicalMessageByIdQuery query, CancellationToken cancellationToken = default);
}