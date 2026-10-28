namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;
/*
 * Represents the duration of therapeutic plan measured in weeks
 */

public sealed record PlanDuration
{
    public const int MinWeeks = 1;
    public const int MaxWeeks = 52;
    
    public int Weeks { get; }

    public PlanDuration(int weeks)
    {
        if (weeks is < MinWeeks or > MaxWeeks)
            throw new ArgumentOutOfRangeException(nameof(weeks), weeks, $"Plan duration must be between {MinWeeks} and {MaxWeeks} weeks.");

        Weeks = weeks;
    }
    public int TotalDays => Weeks * 7;

    public override string ToString() => Weeks == 1 ? "1 week" : $"{Weeks} weeks";
    public static implicit operator int (PlanDuration duration) => duration.Weeks;
}