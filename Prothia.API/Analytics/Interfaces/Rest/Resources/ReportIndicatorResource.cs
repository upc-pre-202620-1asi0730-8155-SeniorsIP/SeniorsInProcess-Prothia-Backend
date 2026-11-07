using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Analytics.Interfaces.Rest.Resources;

public record ReportIndicatorResource(
    [Required(ErrorMessage = "RequiredIndicatorName")]
    string Name,
    [Required(ErrorMessage = "RequiredIndicatorValue")]
    string Value,
    [Required(ErrorMessage = "RequiredIndicatorNormalRange")]
    string NormalRange,
    [Required(ErrorMessage = "RequiredIndicatorEvaluation")]
    string Evaluation,
    [Required(ErrorMessage = "RequiredIndicatorStatus")]
    string Status);