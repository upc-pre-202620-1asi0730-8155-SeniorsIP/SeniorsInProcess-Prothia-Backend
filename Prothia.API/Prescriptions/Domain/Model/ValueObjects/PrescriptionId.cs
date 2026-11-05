using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

public readonly record struct PrescriptionId(int Value) : IEntityId<int>
{
    public static implicit operator int(PrescriptionId id) => id.Value;
    public static implicit operator PrescriptionId(int value) => new(value);

    public override string ToString() => Value.ToString();
}