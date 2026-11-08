using System.ComponentModel.DataAnnotations;
using Prothia.API.Monitoring.Domain.Model.Aggregates;

namespace Prothia.API.Monitoring.Interfaces.Rest.Resources;

public record UpdateThresholdProfileResource(
    [Range(ThresholdProfile.PelvicTiltMin, ThresholdProfile.PelvicTiltMax, ErrorMessage = "InvalidPelvicTilt")]
    int PelvicTilt,
    [Range(ThresholdProfile.AsymmetryMin, ThresholdProfile.AsymmetryMax, ErrorMessage = "InvalidAsymmetry")]
    int Asymmetry,
    [Range(ThresholdProfile.CadenceMin, ThresholdProfile.CadenceMax, ErrorMessage = "InvalidCadence")]
    int Cadence,
    [Range(ThresholdProfile.ImpactPeakMin, ThresholdProfile.ImpactPeakMax, ErrorMessage = "InvalidImpactPeak")]
    double ImpactPeak);
