using Prothia.API.Shared.Domain.Model.Aggregates.Internal;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Aggregates;

public partial class MechanicalAlert : AggregateRoot
{
    public const int PatientNameMaxLength = 150;
    public const int ComponentMaxLength = 150;
    public const int DescriptionMaxLength = 500;
    public const int OccurredAtMaxLength = 100;

    public MechanicalAlertId Id { get; private set; }
    public int ProsthesisId { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public int Cycles { get; private set; }
    public int CycleLimit { get; private set; }
    public string PatientName { get; private set; }
    public WorkshopKLevel KLevel { get; private set; }
    public string Component { get; private set; }
    public string Description { get; private set; }
    public MechanicalAlertSeverity Severity { get; private set; }
    public MechanicalAlertStatus Status { get; private set; }
    public string OccurredAt { get; private set; }

    public bool IsResolved => Status == MechanicalAlertStatus.Resolved;

    protected MechanicalAlert()
    {
        SerialNumber = null!;
        PatientName = string.Empty;
        Component = string.Empty;
        Description = string.Empty;
        OccurredAt = string.Empty;
    }

    public MechanicalAlert(CreateMechanicalAlertCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.SerialNumber);
        if (command.ProsthesisId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.ProsthesisId,
                "ProsthesisId must be greater than zero.");
        if (command.Cycles < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.Cycles, "Cycles cannot be negative.");
        if (command.CycleLimit <= 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.CycleLimit,
                "CycleLimit must be greater than zero.");
        if (string.IsNullOrWhiteSpace(command.Component))
            throw new ArgumentException("Component cannot be null or whitespace.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(command));
        if (!Enum.IsDefined(command.KLevel))
            throw new ArgumentOutOfRangeException(nameof(command), command.KLevel, "Invalid K-level.");
        if (!Enum.IsDefined(command.Severity))
            throw new ArgumentOutOfRangeException(nameof(command), command.Severity, "Invalid alert severity.");

        ProsthesisId = command.ProsthesisId;
        SerialNumber = command.SerialNumber;
        Cycles = command.Cycles;
        CycleLimit = command.CycleLimit;
        PatientName = (command.PatientName ?? string.Empty).Trim();
        KLevel = command.KLevel;
        Component = command.Component.Trim();
        Description = command.Description.Trim();
        Severity = command.Severity;
        Status = MechanicalAlertStatus.Pending;
        OccurredAt = string.IsNullOrWhiteSpace(command.OccurredAt)
            ? DateTimeOffset.UtcNow.ToString("O")
            : command.OccurredAt.Trim();
    }
    
    public void Resolve()
    {
        if (IsResolved)
            throw new InvalidOperationException("The mechanical alert is already resolved.");
        Status = MechanicalAlertStatus.Resolved;
    }
}
