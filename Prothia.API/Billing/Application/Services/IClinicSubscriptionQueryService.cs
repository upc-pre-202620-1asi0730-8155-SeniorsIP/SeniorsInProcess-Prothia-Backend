using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Queries;

namespace Prothia.API.Billing.Application.Services;

public interface IClinicSubscriptionQueryService
{
    Task<IEnumerable<ClinicSubscription>> Handle(GetAllClinicSubscriptionsQuery query,
        CancellationToken cancellationToken = default);

    Task<ClinicSubscription?> Handle(GetClinicSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default);
}