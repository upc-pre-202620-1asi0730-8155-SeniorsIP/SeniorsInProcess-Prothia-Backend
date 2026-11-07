using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public readonly record struct ClinicSubscriptionId(int Value) : IEntityId<int>
{
    public static ClinicSubscriptionId Empty => new(0);
    public static implicit operator int(ClinicSubscriptionId id) => id.Value;
    public static implicit operator ClinicSubscriptionId(int value) => new(value);
    public override string ToString() => Value.ToString();
}