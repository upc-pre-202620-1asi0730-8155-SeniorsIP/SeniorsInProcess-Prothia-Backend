namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record MechanicalAlertResource(
    int Id,
    int ProsthesisId,
    string SerialNumber,
    int Cycles,
    int CycleLimit,
    string PatientName,
    string KLevel,
    string Component,
    string Description,
    string Severity,
    string Status,
    string OccurredAt);