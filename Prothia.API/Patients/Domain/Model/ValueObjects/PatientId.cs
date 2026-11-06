using Prothia.API.Shared.Domain.Model.ValueObjects;
namespace Prothia.API.Patients.Domain.Model.ValueObjects;

public readonly record struct PatientId(int Value) : IEntityId<int>
{
    public static implicit operator int(PatientId id) => id.Value;
    public static implicit operator PatientId(int value) => new(value);

    public override string ToString() => Value.ToString();
}