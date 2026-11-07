using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;

namespace Prothia.API.Monitoring.Domain.Model.Aggregates;

public partial class ThresholdProfile : AggregateRoot
{
    public const int PelvicTiltMin = 0;
    public const int PelvicTiltMax = 30;
    public const int AsymmetryMin = 0;
    public const int AsymmetryMax = 40;
    public const int CadenceMin = 40;
    public const int CadenceMax = 140;
    public const double ImpactPeakMin = 0.5;
    public const double ImpactPeakMax = 3.0;

    public ThresholdProfileId Id { get; private set; }
    public PatientId PatientId { get; private set; }
    
    public int PelvicTilt { get; private set; }
    
    public int Asymmetry { get; private set; }
    
    public int Cadence { get; private set; }
    
    public double ImpactPeak { get; private set; }

    protected ThresholdProfile()
    {
    }

    public ThresholdProfile(CreateThresholdProfileCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        PatientId = command.PatientId;
        Calibrate(command.PelvicTilt, command.Asymmetry, command.Cadence, command.ImpactPeak);
    }
    
    public void Update(UpdateThresholdProfileCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Calibrate(command.PelvicTilt, command.Asymmetry, command.Cadence, command.ImpactPeak);
    }

    private void Calibrate(int pelvicTilt, int asymmetry, int cadence, double impactPeak)
    {
        if (pelvicTilt is < PelvicTiltMin or > PelvicTiltMax)
            throw new ArgumentOutOfRangeException(nameof(pelvicTilt), pelvicTilt,
                $"PelvicTilt must be between {PelvicTiltMin} and {PelvicTiltMax}.");
        if (asymmetry is < AsymmetryMin or > AsymmetryMax)
            throw new ArgumentOutOfRangeException(nameof(asymmetry), asymmetry,
                $"Asymmetry must be between {AsymmetryMin} and {AsymmetryMax}.");
        if (cadence is < CadenceMin or > CadenceMax)
            throw new ArgumentOutOfRangeException(nameof(cadence), cadence,
                $"Cadence must be between {CadenceMin} and {CadenceMax}.");
        if (double.IsNaN(impactPeak) || impactPeak is < ImpactPeakMin or > ImpactPeakMax)
            throw new ArgumentOutOfRangeException(nameof(impactPeak), impactPeak,
                $"ImpactPeak must be between {ImpactPeakMin} and {ImpactPeakMax}.");

        PelvicTilt = pelvicTilt;
        Asymmetry = asymmetry;
        Cadence = cadence;
        ImpactPeak = impactPeak;
    }
}