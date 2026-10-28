using Humanizer;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;
/*
 * Represents the patient's daily walking step target prescribed in their rehabilitation routine.
 */

public sealed record DailyStepGoal
{
    public const int MinSteps = 500;
    public const int MaxSteps =50_000;
    public const int DefaultGoal = 5_000;
    
    
    public int TargetSteps {get;}

    public DailyStepGoal(int targetSteps = DefaultGoal)
    {
        if (targetSteps is < MinSteps or > MaxSteps)
            throw new ArgumentOutOfRangeException(nameof(targetSteps), targetSteps, $"Daily step target must be between {MinSteps:N0} and {MaxSteps:N0} steps.");
        TargetSteps = targetSteps;
    }

    public int CalulateProgressPercentage(int currentSteps)
    {
        if (currentSteps<=0 ) return 0;
        return Math.Min(100, (int)Math.Round((double)currentSteps / TargetSteps * 100));
    }
    public override string ToString() => $"{TargetSteps:N0} steps/day";

    public static implicit operator int(DailyStepGoal goal) => goal.TargetSteps;
}