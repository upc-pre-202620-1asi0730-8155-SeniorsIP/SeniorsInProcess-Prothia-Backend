using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.QueryServices;

public class ProsthesisQueryService(IProsthesisRepository prosthesisRepository) : IProsthesisQueryService
{
    public async Task<IEnumerable<Prosthesis>> Handle(GetAllProsthesesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await prosthesisRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<Prosthesis?> Handle(GetProsthesisByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await prosthesisRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}