using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Commands;

public record CreateMechanicalAlertCommand(
    int ProsthesisId,
    SerialNumber SerialNumber,
    int Cycles,
    int CycleLimit,
    string PatientName,
    WorkshopKLevel KLevel,
    string Component,
    string Description,
    MechanicalAlertSeverity Severity,
    string OccurredAt);