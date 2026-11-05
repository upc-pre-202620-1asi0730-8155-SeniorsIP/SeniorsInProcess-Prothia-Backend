using Prothia.API.Billing.Application.Services;
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
        return await clinicSubscriptionRepository.ListAsync(cancellationToken);
    }

    public async Task<ClinicSubscription?> Handle(GetClinicSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await clinicSubscriptionRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}