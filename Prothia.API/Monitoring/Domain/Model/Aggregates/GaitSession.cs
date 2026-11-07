using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Aggregates;

public partial class GaitSession : AggregateRoot
{
    public GaitSessionId Id { get; private set; }
    public PatientId PatientId { get; private set; }
    public DateTimeOffset RecordedAt { get; private set; }
    
    public int DurationMinutes { get; private set; }
    
    public int Steps { get; private set; }
    
    public int Cadence { get; private set; }
    
    public int SymmetryPercent { get; private set; }
    
    public int AlertsCount { get; private set; }
    
    public int ImpactPeakNewtons { get; private set; }
    
    public SymmetryGrade SymmetryGrade => SymmetryPercent switch
    {
        >= 92 => SymmetryGrade.Excellent,
        >= 90 => SymmetryGrade.Optimal,
        >= 85 => SymmetryGrade.Acceptable,
        _ => SymmetryGrade.NeedsAttention
    };

    protected GaitSession()
    {
    }

    public GaitSession(RecordGaitSessionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.DurationMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.DurationMinutes,
                "DurationMinutes must be greater than zero.");
        if (command.Steps < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.Steps, "Steps cannot be negative.");
        if (command.Cadence < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.Cadence, "Cadence cannot be negative.");
        if (command.SymmetryPercent is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(command), command.SymmetryPercent,
                "SymmetryPercent must be between 0 and 100.");
        if (command.AlertsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.AlertsCount,
                "AlertsCount cannot be negative.");
        if (command.ImpactPeakNewtons < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.ImpactPeakNewtons,
                "ImpactPeakNewtons cannot be negative.");

        PatientId = command.PatientId;
        RecordedAt = command.RecordedAt;
        DurationMinutes = command.DurationMinutes;
        Steps = command.Steps;
        Cadence = command.Cadence;
        SymmetryPercent = command.SymmetryPercent;
        AlertsCount = command.AlertsCount;
        ImpactPeakNewtons = command.ImpactPeakNewtons;
    }
}
