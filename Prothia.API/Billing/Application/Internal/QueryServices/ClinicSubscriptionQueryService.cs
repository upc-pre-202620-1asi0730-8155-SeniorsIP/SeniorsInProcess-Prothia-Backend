using Prothia.API.Billing.Application.QueryServices;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Queries;
using Prothia.API.Billing.Domain.Repositories;

namespace Prothia.API.Billing.Application.Internal.QueryServices;

public class ClinicSubscriptionQueryService(IClinicSubscriptionRepository clinicSubscriptionRepository)
    : IClinicSubscriptionQueryService
{
    public async Task<IEnumerable<ClinicSubscription>> Handle(GetAllClinicSubscriptionsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicSubscriptionRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<ClinicSubscription?> Handle(GetClinicSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicSubscriptionRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}