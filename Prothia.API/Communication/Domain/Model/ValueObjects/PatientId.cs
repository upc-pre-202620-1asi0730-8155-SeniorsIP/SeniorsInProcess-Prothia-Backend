using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
/// Strongly-typed cross-context reference to a Patient in the Patients bounded context.
/// </summary>
/// <param name="Id">The underlying integer patient identifier.</param>
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