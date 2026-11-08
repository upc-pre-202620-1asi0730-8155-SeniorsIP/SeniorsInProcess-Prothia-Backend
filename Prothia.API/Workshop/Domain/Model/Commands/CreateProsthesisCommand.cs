using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Commands;


public record CreateProsthesisCommand(
    SerialNumber SerialNumber,
    string Type,
    string PatientName,
    string ClinicName,
    int AccumulatedCycles,
    int CycleLimit,
    string LastService,
    ProsthesisStatus Status,
    int BatteryLevel,
    SensorStatus SensorStatus,
    WorkshopKLevel KLevel,
    string SocketType,
    string KneeType,
    string FootType);