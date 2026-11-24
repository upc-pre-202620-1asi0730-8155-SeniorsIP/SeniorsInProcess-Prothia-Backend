using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Iam.Domain.Model.ValueObjects;

public readonly record struct UserId(int Value) : IEntityId<int>
{
    public static implicit operator int(UserId id) => id.Value;
    public static implicit operator UserId(int value) => new(value);

    public override string ToString() => Value.ToString();
}