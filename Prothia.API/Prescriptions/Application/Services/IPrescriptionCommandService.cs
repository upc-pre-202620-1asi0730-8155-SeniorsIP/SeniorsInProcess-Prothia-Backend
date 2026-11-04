using Prothia.API.Prescriptions.Application.Errors;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.Queries;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Prescriptions.Application.Services;

public interface IPrescriptionCommandService
{
    Task<Result<Prescription, ECreatePrescriptionError>> Handle(CreatePrescriptionCommand command, 
        CancellationToken cancellationToken = default);
    Task<Result<Prescription, EUpdatePrescriptionError>> Handle(UpdatePrescriptionCommand command, 
        CancellationToken cancellationToken = default);
}