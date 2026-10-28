using ZstdSharp.Unsafe;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents the training frequency per week of a prescription plan (1 to 7 sessions per week).
/// </summary>
public sealed record WeeklyFrequency
{
    public const int MinDays = 1;
    public const int MaxDays = 7;
    
    public int  DaysPerWeek { get; }

    public WeeklyFrequency(int daysPerWeek)
    {
        if (daysPerWeek is < MinDays or > MaxDays)
            throw new ArgumentOutOfRangeException(nameof(daysPerWeek), daysPerWeek,
                $"Weekly frequency must be between {{MinDays}} and {{MaxDays}} days per week.");
        DaysPerWeek = daysPerWeek;
    }
    public override string ToString() => DaysPerWeek==1 ? "1 session per week" : $"{DaysPerWeek} sessions per week";
    
    public static implicit operator int (WeeklyFrequency frequency) => frequency.DaysPerWeek;
}