using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.Entities;


public class ReportIndicator
{
    public const int NameMaxLength = 100;
    public const int ValueMaxLength = 100;
    public const int NormalRangeMaxLength = 100;
    public const int EvaluationMaxLength = 250;

    public int Id { get; private set; }
    public int ClinicalReportId { get; private set; }
    public string Name { get; private set; }
    public string Value { get; private set; }
    public string NormalRange { get; private set; }
    public string Evaluation { get; private set; }
    public IndicatorStatus Status { get; private set; }

    protected ReportIndicator()
    {
        Name = string.Empty;
        Value = string.Empty;
        NormalRange = string.Empty;
        Evaluation = string.Empty;
    }

    public ReportIndicator(string name, string value, string normalRange, string evaluation, IndicatorStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Indicator name cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Indicator value cannot be null or whitespace.", nameof(value));
        if (string.IsNullOrWhiteSpace(normalRange))
            throw new ArgumentException("Normal range cannot be null or whitespace.", nameof(normalRange));
        if (string.IsNullOrWhiteSpace(evaluation))
            throw new ArgumentException("Clinical evaluation cannot be null or whitespace.", nameof(evaluation));
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid indicator status.");

        var trimmedName = name.Trim();
        var trimmedValue = value.Trim();
        var trimmedRange = normalRange.Trim();
        var trimmedEvaluation = evaluation.Trim();

        if (trimmedName.Length > NameMaxLength)
            throw new ArgumentException($"Indicator name cannot exceed {NameMaxLength} characters.", nameof(name));
        if (trimmedValue.Length > ValueMaxLength)
            throw new ArgumentException($"Indicator value cannot exceed {ValueMaxLength} characters.", nameof(value));
        if (trimmedRange.Length > NormalRangeMaxLength)
            throw new ArgumentException($"Normal range cannot exceed {NormalRangeMaxLength} characters.",
                nameof(normalRange));
        if (trimmedEvaluation.Length > EvaluationMaxLength)
            throw new ArgumentException($"Evaluation cannot exceed {EvaluationMaxLength} characters.",
                nameof(evaluation));

        Name = trimmedName;
        Value = trimmedValue;
        NormalRange = trimmedRange;
        Evaluation = trimmedEvaluation;
        Status = status;
    }
}
