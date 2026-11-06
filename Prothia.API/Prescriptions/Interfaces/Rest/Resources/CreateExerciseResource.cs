using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to register an exercise in the catalog.
/// </summary>
/// <param name="Name">Name of the exercise (required)</param>
/// <param name="Focus">Clinical focus tags for the exercise (at least one required)</param>
public record CreateExerciseResource(
    [Required(ErrorMessage = "RequiredExerciseName")]
    string Name,
    [Required(ErrorMessage = "RequiredExerciseFocus")]
    [MinLength(1, ErrorMessage = "RequiredExerciseFocus")]
    IReadOnlyList<string> Focus);