using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

public readonly record struct PatientId : IEntityId<int>
{
    public int Id { get; }
    public int Value => Id;

    public PatientId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Patient ID must be greater than zero.", nameof(id));
        Id = id;
    }

    public static implicit operator int(PatientId id) => id.Id;
    public static implicit operator PatientId(int value) => new(value);
    public override string ToString() => Id.ToString();
}