using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Communication.Application.CommandServices;

public interface IClinicalTransmissionCommandService
{
    Task<Result<ClinicalTransmission>> Handle(
        CreateClinicalTransmissionCommand command, CancellationToken cancellationToken = default);
}