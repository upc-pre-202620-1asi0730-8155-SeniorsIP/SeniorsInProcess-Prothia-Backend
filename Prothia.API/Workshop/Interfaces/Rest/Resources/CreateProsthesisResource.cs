using System.ComponentModel.DataAnnotations;
using Prothia.API.Workshop.Domain.Model.Aggregates;

namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record CreateProsthesisResource(
    [Required(ErrorMessage = "RequiredSerialNumber")]
    string SerialNumber,
    [Required(ErrorMessage = "RequiredProsthesisType")]
    string Type,
    string PatientName,
    string ClinicName,
    [Range(0, int.MaxValue, ErrorMessage = "InvalidAccumulatedCycles")]
    int AccumulatedCycles,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidCycleLimit")]
    int CycleLimit,
    string LastService,
    [Required(ErrorMessage = "RequiredProsthesisStatus")]
    string Status,
    [Range(Prosthesis.BatteryLevelMin, Prosthesis.BatteryLevelMax, ErrorMessage = "InvalidBatteryLevel")]
    int BatteryLevel,
    [Required(ErrorMessage = "RequiredSensorStatus")]
    string SensorStatus,
    [Required(ErrorMessage = "RequiredKLevel")]
    string KLevel,
    string SocketType,
    string KneeType,
    string FootType);