using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.ValueObjects;

public readonly record struct MechanicalAlertId(int Value) : IEntityId<int>
{
    public static MechanicalAlertId Empty => new(0);
    public static implicit operator int(MechanicalAlertId id) => id.Value;
    public static implicit operator MechanicalAlertId(int value) => new(value);
    public override string ToString() => Value.ToString();
}