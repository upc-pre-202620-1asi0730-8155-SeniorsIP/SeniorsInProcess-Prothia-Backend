namespace Prothia.API.Prescriptions.Interfaces.Rest.Resources;

/// <summary>
///     Represents an exercise item prescribed within a patient's rehabilitation plan.
/// </summary>
/// <param name="ExerciseId">ID of the exercise in the catalog</param>
/// <param name="Name">Name of the exercise</param>
/// <param name="Sets">Number of sets</param>
/// <param name="Reps">Number of repetitions per set</param>
/// <param name="RestSeconds">Rest time in seconds between sets</param>
public record PrescribedExerciseResource(
    int ExerciseId,
    string Name,
    int? Sets = null,
    int? Reps = null,
    int? RestSeconds = null);