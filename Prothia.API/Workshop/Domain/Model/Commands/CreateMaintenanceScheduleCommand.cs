using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Commands;

public record CreateMaintenanceScheduleCommand(
    SerialNumber SerialNumber,
    string PatientName,
    string ScheduledDate,
    string InterventionType,
    string TechnicianName,
    MaintenanceStatus Status,
    string Notes);