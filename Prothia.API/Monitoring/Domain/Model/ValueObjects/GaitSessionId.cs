namespace Prothia.API.Monitoring.Domain.Model.ValueObjects;

public readonly record struct GaitSessionId(int Value) : IEntityId<int>
{
    public static implicit operator int(GaitSessionId id) => id.Value;
    public static implicit operator GaitSessionId(int value) => new(value);

    public override string ToString() => Value.ToString();
}