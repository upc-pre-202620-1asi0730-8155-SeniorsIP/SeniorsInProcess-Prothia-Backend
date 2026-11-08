using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateExerciseCommand from a CreateExerciseResource.
/// </summary>
public static class CreateExerciseCommandFromResourceAssembler
{
    public static CreateExerciseCommand ToCommandFromResource(CreateExerciseResource resource) =>
        new(new ExerciseName(resource.Name), resource.Focus ?? []);
}