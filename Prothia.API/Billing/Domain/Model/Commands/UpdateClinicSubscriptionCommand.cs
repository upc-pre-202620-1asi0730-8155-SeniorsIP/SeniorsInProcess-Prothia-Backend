using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Commands;

public record UpdateClinicSubscriptionCommand(
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
    PaymentMethod PaymentMethod);