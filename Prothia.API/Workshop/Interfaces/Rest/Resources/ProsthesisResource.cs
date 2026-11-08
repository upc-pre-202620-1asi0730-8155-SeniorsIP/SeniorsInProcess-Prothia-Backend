namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record ProsthesisResource(
    int Id,
    string SerialNumber,
    string Type,
    string PatientName,
    string ClinicName,
    int AccumulatedCycles,
    int CycleLimit,
    string LastService,
    string Status,
    int BatteryLevel,
    string SensorStatus,
    string KLevel,
    string SocketType,
    string KneeType,
    string FootType);