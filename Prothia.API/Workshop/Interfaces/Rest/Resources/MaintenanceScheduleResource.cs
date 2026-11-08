namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record MaintenanceScheduleResource(
    int Id,
    string SerialNumber,
    string PatientName,
    string ScheduledDate,
    string InterventionType,
    string TechnicianName,
    string Status,
    string Notes);