using Prothia.API.Shared.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.ValueObjects;

public readonly record struct MaintenanceScheduleId(int Value) : IEntityId<int>
{
    public static MaintenanceScheduleId Empty => new(0);
    public static implicit operator int(MaintenanceScheduleId id) => id.Value;
    public static implicit operator MaintenanceScheduleId(int value) => new(value);
    public override string ToString() => Value.ToString();
}