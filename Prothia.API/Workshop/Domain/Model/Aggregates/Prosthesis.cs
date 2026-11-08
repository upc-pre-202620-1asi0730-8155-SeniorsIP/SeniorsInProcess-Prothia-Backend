using Prothia.API.Shared.Domain.Model.Aggregates.Internal;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Aggregates;

public partial class Prosthesis : AggregateRoot
{
    public const int TypeMaxLength = 100;
    public const int PatientNameMaxLength = 150;
    public const int ClinicNameMaxLength = 150;
    public const int LastServiceMaxLength = 50;
    public const int ComponentTypeMaxLength = 100;
    public const int BatteryLevelMin = 0;
    public const int BatteryLevelMax = 100;

    public ProsthesisId Id { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public string Type { get; private set; }
    public string PatientName { get; private set; }
    public string ClinicName { get; private set; }
    public int AccumulatedCycles { get; private set; }
    public int CycleLimit { get; private set; }
    public string LastService { get; private set; }
    public ProsthesisStatus Status { get; private set; }
    public int BatteryLevel { get; private set; }
    public SensorStatus SensorStatus { get; private set; }
    public WorkshopKLevel KLevel { get; private set; }
    public string SocketType { get; private set; }
    public string KneeType { get; private set; }
    public string FootType { get; private set; }

    protected Prosthesis()
    {
        SerialNumber = null!;
        Type = string.Empty;
        PatientName = string.Empty;
        ClinicName = string.Empty;
        LastService = string.Empty;
        SocketType = string.Empty;
        KneeType = string.Empty;
        FootType = string.Empty;
    }

    public Prosthesis(CreateProsthesisCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.SerialNumber);
        if (string.IsNullOrWhiteSpace(command.Type))
            throw new ArgumentException("Type cannot be null or whitespace.", nameof(command));
        if (command.AccumulatedCycles < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.AccumulatedCycles,
                "AccumulatedCycles cannot be negative.");
        if (command.CycleLimit <= 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.CycleLimit,
                "CycleLimit must be greater than zero.");
        if (command.BatteryLevel is < BatteryLevelMin or > BatteryLevelMax)
            throw new ArgumentOutOfRangeException(nameof(command), command.BatteryLevel,
                $"BatteryLevel must be between {BatteryLevelMin} and {BatteryLevelMax}.");
        if (!Enum.IsDefined(command.Status))
            throw new ArgumentOutOfRangeException(nameof(command), command.Status, "Invalid prosthesis status.");
        if (!Enum.IsDefined(command.SensorStatus))
            throw new ArgumentOutOfRangeException(nameof(command), command.SensorStatus, "Invalid sensor status.");
        if (!Enum.IsDefined(command.KLevel))
            throw new ArgumentOutOfRangeException(nameof(command), command.KLevel, "Invalid K-level.");

        SerialNumber = command.SerialNumber;
        Type = command.Type.Trim();
        PatientName = (command.PatientName ?? string.Empty).Trim();
        ClinicName = (command.ClinicName ?? string.Empty).Trim();
        AccumulatedCycles = command.AccumulatedCycles;
        CycleLimit = command.CycleLimit;
        LastService = (command.LastService ?? string.Empty).Trim();
        Status = command.Status;
        BatteryLevel = command.BatteryLevel;
        SensorStatus = command.SensorStatus;
        KLevel = command.KLevel;
        SocketType = (command.SocketType ?? string.Empty).Trim();
        KneeType = (command.KneeType ?? string.Empty).Trim();
        FootType = (command.FootType ?? string.Empty).Trim();
    }
}
