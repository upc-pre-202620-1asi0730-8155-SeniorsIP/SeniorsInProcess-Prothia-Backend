using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

public readonly record struct ClinicalReportId(int Value) : IEntityId<int>
{
    public static ClinicalReportId Empty => new(0);
    public static implicit operator int(ClinicalReportId id) => id.Value;
    public static implicit operator ClinicalReportId(int value) => new(value);
    public override string ToString() => Value.ToString();
}