namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public record SubscriptionPeriod
{
    public DateOnly StartDate { get; }
    
    public DateOnly RenewalDate { get; }

    public SubscriptionPeriod(DateOnly startDate, DateOnly renewalDate)
    {
        if(startDate >= renewalDate)
            throw new ArgumentException($"Start date '{startDate:dd/MM/yyyy}' must be before renewal date '{renewalDate:dd/MM/yyy}'.", nameof(startDate));
        StartDate = startDate;
        RenewalDate = renewalDate;
    }
    
    public int TotalDays => RenewalDate.DayNumber - StartDate.DayNumber;
    
    public bool IsActive(DateOnly date) => date >= StartDate && date <= RenewalDate;

    public int RemainingDays(DateOnly date) => Math.Max(0, RenewalDate.DayNumber - date.DayNumber);

    public override string ToString() => $"{StartDate:dd/MM/yyyy} - {RenewalDate:dd/MM/yyyy}";
}