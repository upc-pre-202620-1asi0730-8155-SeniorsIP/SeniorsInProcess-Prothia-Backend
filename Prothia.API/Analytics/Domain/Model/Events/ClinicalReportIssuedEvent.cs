using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Analytics.Domain.Model.Events;

public record ClinicalReportIssuedEvent(string ReportCode, int PatientId, string ClinicName) : IEvent;