using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

public readonly record struct ReportIndicatorId(int Value) : IEntityId<int>
{
    public static ReportIndicatorId Empty => new(0);
    public static implicit operator int(ReportIndicatorId id) => id.Value;
    public static implicit operator ReportIndicatorId(int value) => new(value);
    public override string ToString() => Value.ToString();
}