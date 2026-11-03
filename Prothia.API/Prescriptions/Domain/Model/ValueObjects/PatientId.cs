namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
/// Reference to a patient owned by the Patients bounded context. Prescriptions only keeps the identifier.
/// </summary>
public sealed record PatientId
{
    public int Value { get; }

    public PatienId(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "PatientId must be greater than zero.");
        Value = value;
    }
    public override string ToString() => Value.ToString();
}