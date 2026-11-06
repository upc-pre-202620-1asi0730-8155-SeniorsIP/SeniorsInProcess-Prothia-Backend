using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Commands;

/// <summary>
///     Command to register a new exercise in the rehabilitation catalog.
/// </summary>
public record CreateExerciseCommand(ExerciseName Name, IReadOnlyList<string> Focus);
