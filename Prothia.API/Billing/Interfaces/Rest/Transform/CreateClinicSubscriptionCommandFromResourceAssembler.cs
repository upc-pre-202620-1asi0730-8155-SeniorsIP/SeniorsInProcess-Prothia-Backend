using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Prothia.API.Billing.Interfaces.Rest.Resources;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateClinicSubscriptionCommand from a CreateClinicSubscriptionResource.
/// </summary>
public static class CreateClinicSubscriptionCommandFromResourceAssembler
{
    public static CreateClinicSubscriptionCommand ToCommandFromResource(CreateClinicSubscriptionResource resource) =>
        new(
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