using Prothia.API.Communication.Application.CommandServices;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling clinical message commands.
/// </summary>
public class ClinicalMessageCommandService(
    IClinicalMessageRepository clinicalMessageRepository,
    IUnitOfWork unitOfWork)
    : IClinicalMessageCommandService
{
    /// <inheritdoc />
    public async Task<Result<ClinicalMessage>> Handle(CreateClinicalMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var message = new ClinicalMessage(command);
        await clinicalMessageRepository.AddAsync(message, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ClinicalMessage>.Success(message);
    }
}