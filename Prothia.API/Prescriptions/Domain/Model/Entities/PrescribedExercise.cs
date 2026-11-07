using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Entities;

/// <summary>
///     Child entity representing a single exercise and its dosage inside a <see cref="Aggregates.Prescription" />.
/// </summary>
public partial class PrescribedExercise
{
    public const int NameMaxLength = 150;
    public const int SetsMax = 20;
    public const int RepsMax = 100;
    public const int RestSecondsMax = 600;

    public PrescribedExerciseId Id { get; }
    public PrescriptionId PrescriptionId { get; private set; }
    public ExerciseId ExerciseId { get; private set; }
    public string Name { get; private set; }
    public int? Sets { get; private set; }
    public int? Reps { get; private set; }
    public int? RestSeconds { get; private set; }

    protected PrescribedExercise()
    {
        Name = string.Empty;
    }

    public PrescribedExercise(PrescribedExerciseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.ExerciseId <= 0)
            throw new ArgumentOutOfRangeException(nameof(item), item.ExerciseId, "ExerciseId must be greater than zero.");
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ArgumentException("Prescribed exercise name cannot be null or whitespace.", nameof(item));
        var trimmedName = item.Name.Trim();
        if (trimmedName.Length > NameMaxLength)
            throw new ArgumentException($"Prescribed exercise name cannot exceed {NameMaxLength} characters.",
                nameof(item));

        if (item.Sets is <= 0 or > SetsMax)
            throw new ArgumentOutOfRangeException(nameof(item), item.Sets,
                $"Sets must be between 1 and {SetsMax}.");
        if (item.Reps is <= 0 or > RepsMax)
            throw new ArgumentOutOfRangeException(nameof(item), item.Reps,
                $"Reps must be between 1 and {RepsMax}.");
        if (item.RestSeconds is < 0 or > RestSecondsMax)
            throw new ArgumentOutOfRangeException(nameof(item), item.RestSeconds,
                $"RestSeconds must be between 0 and {RestSecondsMax}.");

        ExerciseId = new ExerciseId(item.ExerciseId);
        Name = trimmedName;
        Sets = item.Sets;
        Reps = item.Reps;
        RestSeconds = item.RestSeconds;
    }
}
