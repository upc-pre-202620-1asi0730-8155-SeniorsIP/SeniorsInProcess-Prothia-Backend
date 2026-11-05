using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Aggregates;


public partial class ClinicSubscription
{
    public const int PlanNameMaxLength = 150;
    public const int StatusMaxLength = 50;
    public const int PlanTypeMaxLength = 150;
    public const int DateMaxLength = 30;

    public int Id { get; private set; }
    public string PlanName { get; private set; }
    public string Status { get; private set; }
    public string PlanType { get; private set; }
    public decimal AnnualFee { get; private set; }
    public decimal MonthlyEquivalent { get; private set; }
    public string StartDate { get; private set; }
    public string RenewalDate { get; private set; }
    public int UsedQuota { get; private set; }
    public int TotalQuota { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    protected ClinicSubscription()
    {
        PlanName = string.Empty;
        Status = string.Empty;
        PlanType = string.Empty;
        StartDate = string.Empty;
        RenewalDate = string.Empty;
        PaymentMethod = null!;
    }

    public ClinicSubscription(CreateClinicSubscriptionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        PlanName = string.Empty;
        Status = string.Empty;
        PlanType = string.Empty;
        StartDate = string.Empty;
        RenewalDate = string.Empty;
        PaymentMethod = null!;

        ApplyValues(
            command.PlanName,
            command.Status,
            command.PlanType,
            command.AnnualFee,
            command.MonthlyEquivalent,
            command.StartDate,
            command.RenewalDate,
            command.UsedQuota,
            command.TotalQuota,
            command.PaymentMethod);
    }
    
    public void Update(UpdateClinicSubscriptionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ApplyValues(
            command.PlanName,
            command.Status,
            command.PlanType,
            command.AnnualFee,
            command.MonthlyEquivalent,
            command.StartDate,
            command.RenewalDate,
            command.UsedQuota,
            command.TotalQuota,
            command.PaymentMethod);
    }

    private void ApplyValues(
        string planName,
        string status,
        string planType,
        decimal annualFee,
        decimal monthlyEquivalent,
        string startDate,
        string renewalDate,
        int usedQuota,
        int totalQuota,
        PaymentMethod paymentMethod)
    {
        PlanName = ValidateRequiredString(planName, PlanNameMaxLength, nameof(planName));
        Status = ValidateRequiredString(status, StatusMaxLength, nameof(status));
        PlanType = ValidateRequiredString(planType, PlanTypeMaxLength, nameof(planType));

        if (annualFee < 0)
            throw new ArgumentOutOfRangeException(nameof(annualFee), annualFee, "AnnualFee cannot be negative.");
        if (monthlyEquivalent < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyEquivalent), monthlyEquivalent,
                "MonthlyEquivalent cannot be negative.");

        StartDate = ValidateRequiredString(startDate, DateMaxLength, nameof(startDate));
        RenewalDate = ValidateRequiredString(renewalDate, DateMaxLength, nameof(renewalDate));

        if (totalQuota <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalQuota), totalQuota,
                "TotalQuota must be greater than zero.");
        if (usedQuota < 0 || usedQuota > totalQuota)
            throw new ArgumentOutOfRangeException(nameof(usedQuota), usedQuota,
                "UsedQuota must be between 0 and TotalQuota.");

        AnnualFee = Math.Round(annualFee, 2);
        MonthlyEquivalent = Math.Round(monthlyEquivalent, 2);
        UsedQuota = usedQuota;
        TotalQuota = totalQuota;
        PaymentMethod = paymentMethod ?? throw new ArgumentNullException(nameof(paymentMethod));
    }

    private static string ValidateRequiredString(string? value, int maxLength, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new ArgumentException($"{paramName} cannot exceed {maxLength} characters.", paramName);

        return trimmed;
    }
}
