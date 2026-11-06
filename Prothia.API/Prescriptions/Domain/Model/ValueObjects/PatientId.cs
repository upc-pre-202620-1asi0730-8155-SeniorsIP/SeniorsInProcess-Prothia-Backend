using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

public readonly record struct PatientId : IEntityId<int>
{
    public int Value { get; }

    public PatientId(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "PatientId must be greater than zero.");
        Value = value;
    }

    public static implicit operator int(PatientId id) => id.Value;
    public static implicit operator PatientId(int value) => new(value);

    public override string ToString() => Value.ToString();
}