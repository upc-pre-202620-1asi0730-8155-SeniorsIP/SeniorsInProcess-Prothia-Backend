using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Billing.Application.CommandServices;

public interface IClinicSubscriptionCommandService
{
    Task<Result<ClinicSubscription>> Handle(CreateClinicSubscriptionCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<ClinicSubscription>> Handle(UpdateClinicSubscriptionCommand command,
        CancellationToken cancellationToken = default);
}