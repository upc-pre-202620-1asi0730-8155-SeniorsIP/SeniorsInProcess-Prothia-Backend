using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record CreateMechanicalAlertResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidProsthesisId")]
    int ProsthesisId,
    [Required(ErrorMessage = "RequiredSerialNumber")]
    string SerialNumber,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidAccumulatedCycles")]
    int Cycles,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidCycleLimit")]
    int CycleLimit,
    string PatientName,
    [Required(ErrorMessage = "RequiredKLevel")]
    string KLevel,
    [Required(ErrorMessage = "RequiredMechanicalComponent")]
    string Component,
    [Required(ErrorMessage = "RequiredMechanicalDescription")]
    string Description,
    [Required(ErrorMessage = "RequiredSeverity")]
    string Severity,
    string? OccurredAt = null);