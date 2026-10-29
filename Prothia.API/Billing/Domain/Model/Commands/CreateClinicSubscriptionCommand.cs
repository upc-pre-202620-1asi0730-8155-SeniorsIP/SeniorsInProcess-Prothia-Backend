using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Commands;

public record CreateClinicSubscriptionCommand(
    string PlanName,
    string Status,
    string PLanType,
    decimal AnnualFee,
    decimal MonthlyEquivalent,
    string StartDate,
    string RenewalDate,
    int UsedQuota,
    int TotalQuota,
    PaymentMethod PaymentMethod);