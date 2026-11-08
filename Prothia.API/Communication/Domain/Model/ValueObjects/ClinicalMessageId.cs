using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a ClinicalMessage aggregate root.
/// </summary>
/// <param name="Value">The underlying integer identifier.</param>
public readonly record struct ClinicalMessageId(int Value) : IEntityId<int>
{
    public static ClinicalMessageId Empty => new(0);
    public static implicit operator int(ClinicalMessageId id) => id.Value;
    public static implicit operator ClinicalMessageId(int value) => new(value);
    public override string ToString() => Value.ToString();
}