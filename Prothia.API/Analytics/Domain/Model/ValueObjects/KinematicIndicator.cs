namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
/// Represents an individual biomechanical kinematic indicator in a clinical report
/// (e.g. Cadence, Bilateral Symmetry, Knee Flexion Angle, Lateral Pelvic Tilt).
/// </summary>
public sealed record KinematicIndicator
{
    private const int MaxFieldLength = 100;
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "normal", "warning", "critical"
    };

    public string Name { get; }
    public string MeasuredValue { get; }
    public string NormalRange { get; }
    public string ClinicalEvaluation { get; }
    public string Status { get; }

    public KinematicIndicator(string name, string measuredValue, string normalRange, string clinicalEvaluation, string status = "normal")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Indicator name cannot be null or whitespace.", nameof(name));

        if (string.IsNullOrWhiteSpace(measuredValue))
            throw new ArgumentException("Measured value cannot be null or whitespace.", nameof(measuredValue));

        if (string.IsNullOrWhiteSpace(normalRange))
            throw new ArgumentException("Normal range cannot be null or whitespace.", nameof(normalRange));

        if (string.IsNullOrWhiteSpace(clinicalEvaluation))
            throw new ArgumentException("Clinical evaluation cannot be null or whitespace.", nameof(clinicalEvaluation));

        var trimmedName = name.Trim();
        var trimmedValue = measuredValue.Trim();
        var trimmedRange = normalRange.Trim();
        var trimmedEval = clinicalEvaluation.Trim();
        var normalizedStatus = status.Trim().ToLowerInvariant();

        if (trimmedName.Length > MaxFieldLength)
            throw new ArgumentException($"Indicator name cannot exceed {MaxFieldLength} characters.", nameof(name));

        if (!AllowedStatuses.Contains(normalizedStatus))
            throw new ArgumentException($"Invalid indicator status '{status}'. Allowed values are 'normal', 'warning', 'critical'.", nameof(status));

        Name = trimmedName;
        MeasuredValue = trimmedValue;
        NormalRange = trimmedRange;
        ClinicalEvaluation = trimmedEval;
        Status = normalizedStatus;
    }

    public bool IsNormal => Status == "normal";
    public bool HasAnomaly => Status is "warning" or "critical";

    public string ToCsvRow() => $"\"{Name}\",\"{MeasuredValue}\",\"{NormalRange}\",\"{ClinicalEvaluation}\"";

    public override string ToString() => $"{Name}: {MeasuredValue} (Rango: {NormalRange}) - {ClinicalEvaluation}";
}
