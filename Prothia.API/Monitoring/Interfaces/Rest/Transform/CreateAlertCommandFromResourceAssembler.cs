using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;

namespace Prothia.API.Monitoring.Interfaces.Rest.Transform;

public static class CreateAlertCommandFromResourceAssembler
{
    public static CreateAlertCommand ToCommandFromResource(CreateAlertResource resource) =>
        new(new PatientId(resource.PatientId),
            new DeviationType(resource.DeviationType),
            new ReadingText(resource.RecordedValue),
            new ReadingText(resource.Threshold),
            EnumConversions.Parse<EAlertSeverity>(resource.Severity, nameof(resource.Severity)),
            resource.OccurredAt ?? DateTimeOffset.UtcNow);
}