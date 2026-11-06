using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an ExerciseResource from an Exercise.
/// </summary>
public static class ExerciseResourceFromEntityAssembler
{
    public static ExerciseResource ToResourceFromEntity(Exercise entity) =>
        new(entity.Id.Value, entity.Name.Value, entity.Focus);
}