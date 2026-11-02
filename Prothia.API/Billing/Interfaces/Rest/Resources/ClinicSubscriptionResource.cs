namespace Prothia.API.Billing.Interfaces.Rest.Resources;


public record ClinicSubscriptionResource(
    int Id,
    string PlanName,
    string Status,
    string PlanType,
    decimal AnnualFee,
    decimal MonthlyEquivalent,
    string StartDate,
    string RenewalDate,
    int UsedQuota,
    int TotalQuota,
    PaymentMethodResource PaymentMethod);