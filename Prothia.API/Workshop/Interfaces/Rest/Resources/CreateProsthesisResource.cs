using System.ComponentModel.DataAnnotations;
using Prothia.API.Workshop.Domain.Model.Aggregates;

namespace Prothia.API.Workshop.Interfaces.Rest.Resources;

public record CreateProsthesisResource(
    [Required(ErrorMessage = "RequiredSerialNumber")]
    string SerialNumber,
    [Required(ErrorMessage = "RequiredProsthesisType")]
    string Type,
    string? PatientName = "",
    string? ClinicName = "",
    [Range(0, int.MaxValue, ErrorMessage = "InvalidAccumulatedCycles")]
    int AccumulatedCycles = 0,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidCycleLimit")]
    int CycleLimit = 800000,
    string? LastService = "",
    [Required(ErrorMessage = "RequiredProsthesisStatus")]
    string Status = "optimal",
    [Range(Prosthesis.BatteryLevelMin, Prosthesis.BatteryLevelMax, ErrorMessage = "InvalidBatteryLevel")]
    int BatteryLevel = 100,
    [Required(ErrorMessage = "RequiredSensorStatus")]
    string SensorStatus = "online",
    [Required(ErrorMessage = "RequiredKLevel")]
    string KLevel = "K3",
    string? SocketType = "",
    string? KneeType = "",
    string? FootType = "");
