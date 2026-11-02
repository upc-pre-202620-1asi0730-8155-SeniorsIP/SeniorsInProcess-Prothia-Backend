using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Billing.Interfaces.Rest.Resources;

public record UpdateClinicSubscriptionResource(
    [Required(ErrorMessage = "RequiredSubscriptionPlanName")]
    string PlanName,
    [Required(ErrorMessage = "RequiredSubscriptionStatus")]
    string Status,
    [Required(ErrorMessage = "RequiredSubscriptionPlanType")]
    string PlanType,
    [Range(0, double.MaxValue, ErrorMessage = "InvalidSubscriptionAnnualFee")]
    decimal AnnualFee,
    [Range(0, double.MaxValue, ErrorMessage = "InvalidSubscriptionMonthlyEquivalent")]
    decimal MonthlyEquivalent,
    [Required(ErrorMessage = "RequiredSubscriptionStartDate")]
    string StartDate,
    [Required(ErrorMessage = "RequiredSubscriptionRenewalDate")]
    string RenewalDate,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidSubscriptionUsedQuota")]
    int UsedQuota,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidSubscriptionTotalQuota")]
    int TotalQuota,
    [Required(ErrorMessage = "RequiredSubscriptionPaymentMethod")]
    PaymentMethodResource PaymentMethod);