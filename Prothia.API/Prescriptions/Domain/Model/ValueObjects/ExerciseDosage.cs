namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;
/*
 * Represents the therapeutic dosage of a prescribed exercise (Sets, Repetitions, and Rest in seconds).
 */

public sealed record ExerciseDosage
{
    public const int MaxSets = 20;
    public const int MaxReps = 100;
    public const int MaxRestSeconds = 100;
    
    public int Sets { get; }
    public int Reps { get; }
    public int RestSeconds { get; }
    
    public ExerciseDosage(int  sets, int reps, int restSeconds)
    {
        if (sets<=0 || sets>MaxSets)
            throw new ArgumentOutOfRangeException(nameof(sets), sets, $"Sets must be between 1 and {MaxSets}.");
        if (reps<=0 || reps>MaxReps)
            throw new ArgumentOutOfRangeException(nameof(reps), reps, $"Reps must be between 1 and {MaxReps}.");
        if  (restSeconds<=0 || restSeconds>MaxRestSeconds)
            throw new ArgumentOutOfRangeException(nameof(restSeconds), restSeconds, $"Rest seconds must be between 0 and {MaxRestSeconds}.");
 
        Sets = sets;
        Reps = reps;
        RestSeconds = restSeconds;
    }

    public int TotalRepetitions => Sets * Reps;
    public override string ToString() => $"{Sets} sets x {Reps} reps ({RestSeconds}s rest)";
}