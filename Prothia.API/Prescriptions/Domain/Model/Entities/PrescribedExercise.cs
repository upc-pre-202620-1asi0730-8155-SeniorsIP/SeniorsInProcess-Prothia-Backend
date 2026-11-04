using Prothia.API.Prescriptions.Domain.Model.Commands;

namespace Prothia.API.Prescriptions.Domain.Model.Entities;

/// <summary>
///     Represents an exercise assigned to a specific prescription with its clinical dosage.
/// </summary>
public class PrescribedExercise
{
    public const int NameMaxLength = 150;

    public int Id { get; private set; }
    public int PrescriptionId { get; private set; }
    public int ExerciseId { get; private set; }
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
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(item));
        var trimmedName = item.Name.Trim();
        if (trimmedName.Length > NameMaxLength)
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(item));
        if (item.Sets is <= 0)
            throw new ArgumentOutOfRangeException(nameof(item), item.Sets, "Sets must be greater than zero.");
        if (item.Reps is <= 0)
            throw new ArgumentOutOfRangeException(nameof(item), item.Reps, "Reps must be greater than zero.");
        if (item.RestSeconds is < 0)
            throw new ArgumentOutOfRangeException(nameof(item), item.RestSeconds, "RestSeconds cannot be negative.");

        ExerciseId = item.ExerciseId;
        Name = trimmedName;
        Sets = item.Sets;
        Reps = item.Reps;
        RestSeconds = item.RestSeconds;
    }
}