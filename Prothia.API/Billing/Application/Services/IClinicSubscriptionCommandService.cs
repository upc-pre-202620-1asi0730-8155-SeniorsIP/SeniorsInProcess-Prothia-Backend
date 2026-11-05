using Prothia.API.Billing.Application.Errors;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Billing.Application.Services;

public interface IClinicSubscriptionCommandService
{
    Task<Result<ClinicSubscription, ECreateClinicSubscriptionError>> Handle(CreateClinicSubscriptionCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<ClinicSubscription, EUpdateClinicSubscriptionError>> Handle(UpdateClinicSubscriptionCommand command,
        CancellationToken cancellationToken = default);
}