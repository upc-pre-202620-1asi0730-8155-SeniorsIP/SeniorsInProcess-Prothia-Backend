using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

public readonly record struct ExerciseId(int Value) : IEntityId<int>
{
    public static implicit operator int(ExerciseId id) => id.Value;
    public static implicit operator ExerciseId(int value) => new(value);

    public override string ToString() => Value.ToString();
}