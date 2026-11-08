using Prothia.API.Shared.Application.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;

namespace Prothia.API.Workshop.Application.CommandServices;

public interface IProsthesisCommandService
{
    Task<Result<Prosthesis>> Handle(CreateProsthesisCommand command,
        CancellationToken cancellationToken = default);
}