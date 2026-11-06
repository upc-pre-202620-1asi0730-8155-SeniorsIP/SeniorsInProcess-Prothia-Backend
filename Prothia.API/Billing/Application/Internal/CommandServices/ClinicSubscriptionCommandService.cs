using Prothia.API.Billing.Application.CommandServices;
using Prothia.API.Billing.Domain.Model;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.Errors;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Billing.Application.Internal.CommandServices;

public class ClinicSubscriptionCommandService(
    IClinicSubscriptionRepository clinicSubscriptionRepository,
    IUnitOfWork unitOfWork)
    : IClinicSubscriptionCommandService
{
    public async Task<Result<ClinicSubscription>> Handle(
        CreateClinicSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var subscription = new ClinicSubscription(command);
        await clinicSubscriptionRepository.AddAsync(subscription, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ClinicSubscription>.Success(subscription);
    }

    public async Task<Result<ClinicSubscription>> Handle(
        UpdateClinicSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var subscription = await clinicSubscriptionRepository.FindByIdAsync(command.Id, cancellationToken);
        if (subscription is null)
            return Result<ClinicSubscription>.Failure(BillingErrors.ClinicSubscriptionNotFound,
                BillingError.ClinicSubscriptionNotFound);

        subscription.Update(command);
        clinicSubscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ClinicSubscription>.Success(subscription);
    }
}