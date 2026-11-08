using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Domain.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.Errors;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.CommandServices;

public class MechanicalAlertCommandService(
    IMechanicalAlertRepository mechanicalAlertRepository,
    IProsthesisRepository prosthesisRepository,
    IUnitOfWork unitOfWork)
    : IMechanicalAlertCommandService
{
    public async Task<Result<MechanicalAlert>> Handle(
        CreateMechanicalAlertCommand command, CancellationToken cancellationToken = default)
    {
        var prosthesis = await prosthesisRepository.FindByIdAsync(command.ProsthesisId, cancellationToken);
        if (prosthesis is null)
            return Result<MechanicalAlert>.Failure(WorkshopErrors.ProsthesisNotFound,
                WorkshopError.ProsthesisNotFound);

        var alert = new MechanicalAlert(command);
        await mechanicalAlertRepository.AddAsync(alert, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MechanicalAlert>.Success(alert);
    }

    public async Task<Result<MechanicalAlert>> Handle(
        ResolveMechanicalAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await mechanicalAlertRepository.FindByIdAsync(command.MechanicalAlertId, cancellationToken);
        if (alert is null)
            return Result<MechanicalAlert>.Failure(WorkshopErrors.MechanicalAlertNotFound,
                WorkshopError.MechanicalAlertNotFound);

        if (alert.IsResolved)
            return Result<MechanicalAlert>.Failure(WorkshopErrors.MechanicalAlertAlreadyResolved,
                WorkshopError.MechanicalAlertAlreadyResolved);

        alert.Resolve();
        mechanicalAlertRepository.Update(alert);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MechanicalAlert>.Success(alert);
    }
}
