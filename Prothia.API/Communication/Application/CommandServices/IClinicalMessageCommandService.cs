using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Communication.Application.CommandServices;

public interface IClinicalMessageCommandService
{
    Task<Result<ClinicalMessage>> Handle(CreateClinicalMessageCommand command,
        CancellationToken cancellationToken = default);
}