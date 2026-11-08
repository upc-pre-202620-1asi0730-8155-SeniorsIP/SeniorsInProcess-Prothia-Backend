using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Prescriptions.Application.CommandServices;

public interface IPrescriptionCommandService
{
    Task<Result<Prescription>> Handle(CreatePrescriptionCommand command,
        CancellationToken cancellationToken = default);
    Task<Result<Prescription>> Handle(UpdatePrescriptionCommand command,
        CancellationToken cancellationToken = default);
}