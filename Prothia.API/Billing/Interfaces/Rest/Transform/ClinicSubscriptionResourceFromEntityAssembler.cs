using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Interfaces.Rest.Resources;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class ClinicSubscriptionResourceFromEntityAssembler
{
    public static ClinicSubscriptionResource ToResourceFromEntity(ClinicSubscription entity) =>
        new(
            entity.Id,
            entity.PlanName,
            entity.Status,
            entity.PlanType,
            entity.AnnualFee,
            entity.MonthlyEquivalent,
            entity.StartDate,
            entity.RenewalDate,
            entity.UsedQuota,
            entity.TotalQuota,
            new PaymentMethodResource(
                entity.PaymentMethod.Brand,
                entity.PaymentMethod.CardTitle,
                entity.PaymentMethod.Expiry,
                entity.PaymentMethod.AutoRenew));
}