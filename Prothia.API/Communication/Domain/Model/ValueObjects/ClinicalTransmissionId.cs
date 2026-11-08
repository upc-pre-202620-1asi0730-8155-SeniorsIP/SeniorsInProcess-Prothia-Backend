using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a ClinicalTransmission aggregate root.
/// </summary>
/// <param name="Value">The underlying integer identifier.</param>
public readonly record struct ClinicalTransmissionId(int Value) : IEntityId<int>
{
    public static ClinicalTransmissionId Empty => new(0);
    public static implicit operator int(ClinicalTransmissionId id) => id.Value;
    public static implicit operator ClinicalTransmissionId(int value) => new(value);
    public override string ToString() => Value.ToString();
}