using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.ValueObjects;

public readonly record struct ThresholdProfileId(int Value) : IEntityId<int>
{
    public static implicit operator int(ThresholdProfileId id) => id.Value;
    public static implicit operator ThresholdProfileId(int value) => new(value);

    public override string ToString() => Value.ToString();
}