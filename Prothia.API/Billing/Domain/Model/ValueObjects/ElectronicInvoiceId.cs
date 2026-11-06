using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public readonly record struct ElectronicInvoiceId(int Value) : IEntityId<int>
{
    public static ElectronicInvoiceId Empty => new(0);
    public static implicit operator int(ElectronicInvoiceId id) => id.Value;
    public static implicit operator ElectronicInvoiceId(int value) => new(value);
    public override string ToString() => Value.ToString();
}