using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public static class RecordGaitSessionCommandFromResourceAssembler
{
    public static RecordGaitSessionCommand ToCommandFromResource(RecordGaitSessionResource resource) =>
        new(new PatientId(resource.PatientId),
            resource.RecordedAt ?? DateTimeOffset.UtcNow,
            resource.DurationMinutes,
            resource.Steps,
            resource.Cadence,
            resource.SymmetryPercent,
            resource.AlertsCount,
            resource.ImpactPeakNewtons);
}