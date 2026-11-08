using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.ValueObjects;

public readonly record struct ProsthesisId(int Value) : IEntityId<int>
{
    public static ProsthesisId Empty => new(0);
    public static implicit operator int(ProsthesisId id) => id.Value;
    public static implicit operator ProsthesisId(int value) => new(value);
    public override string ToString() => Value.ToString();
}