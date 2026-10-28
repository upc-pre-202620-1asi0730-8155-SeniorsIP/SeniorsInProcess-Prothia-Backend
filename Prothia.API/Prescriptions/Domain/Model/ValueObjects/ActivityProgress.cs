namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents the completion progress percentage of prescribed therapeutic activities for a given day (0% to 100%).
/// </summary>
public sealed record ActivityProgress
{
    public const int MinValue = 0;
    public const int MaxValue = 100;
    
    public int Percentage { get; }

    public ActivityProgress(int percentage)
    {
        if(percentage is  < MinValue or > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(percentage), percentage, $"Activity progress must be between {MinValue} and {MaxValue}.");
        Percentage = percentage;
    }

    public static ActivityProgress FromCompleted(int completedActivities, int totalActivities)
    {
        if (totalActivities <= 0) return new ActivityProgress(0);
        var pct = (int)Math.Round((double)completedActivities / totalActivities * 100);
        return new ActivityProgress(Math.Clamp(pct, MinValue, MaxValue));
    }
    public bool IsCompleted => Percentage == MaxValue;
    public bool HasStarted => Percentage > 0;
    
    public override string ToString() => $"{Percentage}% completed";
    
    public static implicit operator int(ActivityProgress progress) => progress.Percentage;
}