namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

public sealed record PatientId
{
    public int Value { get; }

    public PatientId(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "PatientId must be greater than zero.");
        Value = value;
    }

    public override string ToString() => Value.ToString();
}