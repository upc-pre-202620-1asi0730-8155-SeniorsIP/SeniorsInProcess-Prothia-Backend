using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Domain.Model;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.Errors;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Application.Internal.CommandServices;

public class ProsthesisCommandService(
    IProsthesisRepository prosthesisRepository,
    IUnitOfWork unitOfWork)
    : IProsthesisCommandService
{
    public async Task<Result<Prosthesis>> Handle(CreateProsthesisCommand command,
        CancellationToken cancellationToken = default)
    {
        var existing = await prosthesisRepository.FindBySerialNumberAsync(command.SerialNumber, cancellationToken);
        if (existing is not null)
            return Result<Prosthesis>.Failure(WorkshopErrors.DuplicateSerialNumber,
                WorkshopError.DuplicateSerialNumber);

        var prosthesis = new Prosthesis(command);
        await prosthesisRepository.AddAsync(prosthesis, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Prosthesis>.Success(prosthesis);
    }

    public async Task<Result<Prosthesis>> AssociateAsync(int id, string? patientName, string? clinicName,
        CancellationToken cancellationToken = default)
    {
        var prosthesis = await prosthesisRepository.FindByIdAsync(id, cancellationToken);
        if (prosthesis is null)
            return Result<Prosthesis>.Failure(WorkshopErrors.ProsthesisNotFound,
                WorkshopError.ProsthesisNotFound);

        prosthesis.Associate(patientName, clinicName);
        prosthesisRepository.Update(prosthesis);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Prosthesis>.Success(prosthesis);
    }
}
