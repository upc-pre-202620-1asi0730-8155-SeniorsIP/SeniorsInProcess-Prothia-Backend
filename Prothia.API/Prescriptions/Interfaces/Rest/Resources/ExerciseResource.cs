namespace Prothia.API.Prescriptions.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an exercise in the catalog.
/// </summary>
/// <param name="Id">The server-generated ID of the exercise</param>
/// <param name="Name">Name of the exercise</param>
/// <param name="Focus">Clinical focus tags for the exercise</param>
public record ExerciseResource(
    int Id,
    string Name,
    IReadOnlyList<string> Focus);