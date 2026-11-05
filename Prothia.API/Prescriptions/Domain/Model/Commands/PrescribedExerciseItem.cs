namespace Prothia.API.Prescriptions.Domain.Model.Commands;

/// <summary>
///     Represents an exercise item within a prescription command.
/// </summary>
public record PrescribedExerciseItem(int ExerciseId, string Name, int? Sets, int? Reps, int? RestSeconds);