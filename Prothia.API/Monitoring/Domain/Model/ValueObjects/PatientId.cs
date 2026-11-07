namespace Prothia.API.Monitoring.Domain.Model.ValueObjects;

public readonly record struct PatientId : IEntityId<int>
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