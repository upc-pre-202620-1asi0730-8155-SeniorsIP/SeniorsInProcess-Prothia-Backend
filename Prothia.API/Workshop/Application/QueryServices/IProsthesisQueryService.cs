using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Queries;

namespace Prothia.API.Workshop.Application.QueryServices;

public interface IProsthesisQueryService
{
    Task<IEnumerable<Prosthesis>> Handle(GetAllProsthesesQuery query, CancellationToken cancellationToken = default);
    Task<Prosthesis?> Handle(GetProsthesisByIdQuery query, CancellationToken cancellationToken = default);
}