using Prothia.API.Analytics.Domain.Model.Events;
using Prothia.API.Shared.Application.Internal.EventHandlers;

namespace Prothia.API.Analytics.Application.Internal.EventHandlers;

public class ClinicalReportIssuedEventHandler(ILogger<ClinicalReportIssuedEventHandler> logger)
    : IEventHandler<ClinicalReportIssuedEvent>
{
    public Task Handle(ClinicalReportIssuedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain Event Handled: Clinical report {ReportCode} issued for PatientId={PatientId} at clinic {ClinicName}",
            domainEvent.ReportCode, domainEvent.PatientId, domainEvent.ClinicName);
        return Task.CompletedTask;
    }
}