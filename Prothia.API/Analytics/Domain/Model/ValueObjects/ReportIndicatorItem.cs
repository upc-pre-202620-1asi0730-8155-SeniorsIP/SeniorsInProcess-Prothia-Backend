namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

public record ReportIndicatorItem(
    string Name,
    string Value,
    string NormalRange,
    string Evaluation,
    IndicatorStatus Status);