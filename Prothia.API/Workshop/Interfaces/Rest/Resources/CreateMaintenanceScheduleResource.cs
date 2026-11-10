using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record CreateMaintenanceScheduleResource(
    [Required(ErrorMessage = "RequiredSerialNumber")]
    string SerialNumber,
    [Required(ErrorMessage = "RequiredFullName")]
    string PatientName,
    [Required(ErrorMessage = "RequiredScheduledDate")]
    string ScheduledDate,
    [Required(ErrorMessage = "RequiredInterventionType")]
    string InterventionType,
    [Required(ErrorMessage = "RequiredTechnicianName")]
    string TechnicianName,
    [Required(ErrorMessage = "RequiredMaintenanceStatus")]
    string Status,
    string? Notes = "");
