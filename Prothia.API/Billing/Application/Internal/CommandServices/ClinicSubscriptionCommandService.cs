using Prothia.API.Billing.Application.Errors;
using Prothia.API.Billing.Application.Services;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Billing.Application.Internal.CommandServices;

public class ClinicSubscriptionCommandService(
    IClinicSubscriptionRepository clinicSubscriptionRepository,
    IUnitOfWork unitOfWork,
    ILogger<ClinicSubscriptionCommandService> logger)
    : IClinicSubscriptionCommandService
{
    /// <inheritdoc />
    public async Task<Result<ClinicSubscription, ECreateClinicSubscriptionError>> Handle(
        CreateClinicSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subscription = new ClinicSubscription(command);
            await clinicSubscriptionRepository.AddAsync(subscription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<ClinicSubscription, ECreateClinicSubscriptionError>.Success(subscription);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating clinic subscription {PlanName}", command.PlanName);
            return new Result<ClinicSubscription, ECreateClinicSubscriptionError>.Failure(
                ECreateClinicSubscriptionError.InvalidClinicSubscription);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating clinic subscription {PlanName}", command.PlanName);
            return new Result<ClinicSubscription, ECreateClinicSubscriptionError>.Failure(
                ECreateClinicSubscriptionError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating clinic subscription {PlanName}", command.PlanName);
            return new Result<ClinicSubscription, ECreateClinicSubscriptionError>.Failure(
                ECreateClinicSubscriptionError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<ClinicSubscription, EUpdateClinicSubscriptionError>> Handle(
        UpdateClinicSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subscription = await clinicSubscriptionRepository.FindByIdAsync(command.Id, cancellationToken);
            if (subscription is null)
                return new Result<ClinicSubscription, EUpdateClinicSubscriptionError>.Failure(
                    EUpdateClinicSubscriptionError.ClinicSubscriptionNotFound);

            subscription.Update(command);
            clinicSubscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<ClinicSubscription, EUpdateClinicSubscriptionError>.Success(subscription);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating clinic subscription {Id}", command.Id);
            return new Result<ClinicSubscription, EUpdateClinicSubscriptionError>.Failure(
                EUpdateClinicSubscriptionError.InvalidClinicSubscription);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating clinic subscription {Id}", command.Id);
            return new Result<ClinicSubscription, EUpdateClinicSubscriptionError>.Failure(
                EUpdateClinicSubscriptionError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating clinic subscription {Id}", command.Id);
            return new Result<ClinicSubscription, EUpdateClinicSubscriptionError>.Failure(
                EUpdateClinicSubscriptionError.UnexpectedError);
        }
    }
}
