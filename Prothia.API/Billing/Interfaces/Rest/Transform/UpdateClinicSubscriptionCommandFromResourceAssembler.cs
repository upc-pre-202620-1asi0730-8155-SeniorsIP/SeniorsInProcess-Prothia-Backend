using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Prothia.API.Billing.Interfaces.Rest.Resources;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class UpdateClinicSubscriptionCommandFromResourceAssembler
{
    public static UpdateClinicSubscriptionCommand ToCommandFromResource(int id,
        UpdateClinicSubscriptionResource resource) =>
        new(
            id,
            resource.PlanName,
            resource.Status,
            resource.PlanType,
            resource.AnnualFee,
            resource.MonthlyEquivalent,
            resource.StartDate,
            resource.RenewalDate,
            resource.UsedQuota,
            resource.TotalQuota,
            new PaymentMethod(
                resource.PaymentMethod.Brand,
                resource.PaymentMethod.CardTitle,
                resource.PaymentMethod.Expiry,
                resource.PaymentMethod.AutoRenew));
}