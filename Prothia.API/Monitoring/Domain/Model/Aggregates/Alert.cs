using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Aggregates;

public partial class Alert
{
    public const int PatientNameMaxLength = 150;
    
    public int Id { get; private set; }
    
    public PatientId PatientId {get; private set;}
    
    public string PatientName {get; private set; }
    
    public DeviationType DeviationType { get; private set; }
    public ReadingText RecordedValue { get; private set; }
    public ReadingText Threshold { get; private set; }
    public EAlertSeverity Severity { get; private set; }
    public EAlertStatus Status { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    
    public bool IsPending => Status == EAlertStatus.Pending;

    protected Alert()
    {
        PatientId = null!;
        PatientName = string.Empty;
        DeviationType = null!;
        RecordedValue = null!;
        Threshold = null!;
    }

    public Alert(CreateAlertCommand command, string patientName)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(patientName))
            throw new ArgumentException("PatientName cannot be null or whitespace.", nameof(patientName));
        if (patientName.Trim().Length > PatientNameMaxLength)
            throw new ArgumentException($"PatientName cannot exceed {PatientNameMaxLength} characters.",
                nameof(patientName));
        if (!Enum.IsDefined(command.Severity))
            throw new ArgumentOutOfRangeException(nameof(command), command.Severity, "Invalid alert severity.");

        PatientId = command.PatientId;
        PatientName = patientName.Trim();
        DeviationType = command.DeviationType;
        RecordedValue = command.RecordedValue;
        Threshold = command.Threshold;
        Severity = command.Severity;
        Status = EAlertStatus.Pending;
        OccurredAt = command.OccurredAt;
    }
    
    public void Resolve(DateTimeOffset resolvedAt)
    {
        if (!IsPending) throw new InvalidOperationException("The alert is already resolved.");
        Status = EAlertStatus.Resolved;
        ResolvedAt = resolvedAt;
    }
}