using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

public readonly record struct PrescribedExerciseId(int Value) : IEntityId<int>
{
    public static implicit operator int(PrescribedExerciseId id) => id.Value;
    public static implicit operator PrescribedExerciseId(int value) => new(value);

    public override string ToString() => Value.ToString();
}