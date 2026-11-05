using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.Commands;


public record ReportIndicatorItem(
    string Name,
    string Value,
    string NormalRange,
    string Evaluation,
    IndicatorStatus Status);