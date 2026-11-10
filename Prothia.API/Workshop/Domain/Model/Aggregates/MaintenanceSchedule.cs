using Prothia.API.Shared.Domain.Model.Aggregates.Internal;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Aggregates;


public partial class MaintenanceSchedule : AggregateRoot
{
    public const int PatientNameMaxLength = 150;
    public const int ScheduledDateMaxLength = 50;
    public const int InterventionTypeMaxLength = 150;
    public const int TechnicianNameMaxLength = 150;
    public const int NotesMaxLength = 500;

    public MaintenanceScheduleId Id { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public string PatientName { get; private set; }
    public string ScheduledDate { get; private set; }
    public string InterventionType { get; private set; }
    public string TechnicianName { get; private set; }
    public MaintenanceStatus Status { get; private set; }
    public string Notes { get; private set; }

    protected MaintenanceSchedule()
    {
        SerialNumber = null!;
        PatientName = string.Empty;
        ScheduledDate = string.Empty;
        InterventionType = string.Empty;
        TechnicianName = string.Empty;
        Notes = string.Empty;
    }

    public MaintenanceSchedule(CreateMaintenanceScheduleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.SerialNumber);
        if (string.IsNullOrWhiteSpace(command.PatientName))
            throw new ArgumentException("PatientName cannot be null or whitespace.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.ScheduledDate))
            throw new ArgumentException("ScheduledDate cannot be null or whitespace.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.InterventionType))
            throw new ArgumentException("InterventionType cannot be null or whitespace.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.TechnicianName))
            throw new ArgumentException("TechnicianName cannot be null or whitespace.", nameof(command));
        if (!Enum.IsDefined(command.Status))
            throw new ArgumentOutOfRangeException(nameof(command), command.Status, "Invalid maintenance status.");

        SerialNumber = command.SerialNumber;
        PatientName = command.PatientName.Trim();
        ScheduledDate = command.ScheduledDate.Trim();
        InterventionType = command.InterventionType.Trim();
        TechnicianName = command.TechnicianName.Trim();
        Status = command.Status;
        Notes = (command.Notes ?? string.Empty).Trim();
    }

    public void Complete(string? notes)
    {
        Status = MaintenanceStatus.Completed;
        if (!string.IsNullOrWhiteSpace(notes))
            Notes = notes.Trim();
    }
}
