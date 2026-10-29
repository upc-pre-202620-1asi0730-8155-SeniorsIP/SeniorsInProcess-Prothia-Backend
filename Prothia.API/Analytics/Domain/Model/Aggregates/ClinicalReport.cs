using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Analytics.Domain.Model.Entities;
using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.Aggregates;


public partial class ClinicalReport
{
    public const int IssueDateMaxLength = 30;
    public const int ClinicNameMaxLength = 150;
    public const int ClinicUnitMaxLength = 150;
    public const int PatientNameMaxLength = 150;
    public const int DniMaxLength = 20;
    public const int DiagnosisMaxLength = 250;
    public const int ProsthesisCodeMaxLength = 50;
    public const int EvaluationPeriodMaxLength = 100;
    public const int SummaryMaxLength = 2000;
    public const int MedicalRecommendationMaxLength = 2000;
    public const int TherapistNameMaxLength = 150;
    public const int TherapistRoleMaxLength = 150;

    public int Id { get; private set; }
    public ReportCode ReportCode { get; private set; }
    public string IssueDate { get; private set; }
    public string ClinicName { get; private set; }
    public string ClinicUnit { get; private set; }
    public PatientId PatientId { get; private set; }
    public string PatientName { get; private set; }
    public string Dni { get; private set; }
    public int Age { get; private set; }
    public string Diagnosis { get; private set; }
    public string ProsthesisCode { get; private set; }
    public string EvaluationPeriod { get; private set; }
    public string Summary { get; private set; }
    public string MedicalRecommendation { get; private set; }
    public string TherapistName { get; private set; }
    public string TherapistRole { get; private set; }
    public TherapistRegistry TherapistRegistry { get; private set; }
    public ICollection<ReportIndicator> Indicators { get; private set; }

    protected ClinicalReport()
    {
        ReportCode = null!;
        IssueDate = string.Empty;
        ClinicName = string.Empty;
        ClinicUnit = string.Empty;
        PatientId = null!;
        PatientName = string.Empty;
        Dni = string.Empty;
        Diagnosis = string.Empty;
        ProsthesisCode = string.Empty;
        EvaluationPeriod = string.Empty;
        Summary = string.Empty;
        MedicalRecommendation = string.Empty;
        TherapistName = string.Empty;
        TherapistRole = string.Empty;
        TherapistRegistry = null!;
        Indicators = new List<ReportIndicator>();
    }

    public ClinicalReport(CreateClinicalReportCommand command, string? resolvedPatientName = null)
    {
        ArgumentNullException.ThrowIfNull(command);

        var effectivePatientName = !string.IsNullOrWhiteSpace(command.PatientName)
            ? command.PatientName
            : resolvedPatientName;

        IssueDate = ValidateRequiredString(command.IssueDate, IssueDateMaxLength, nameof(command.IssueDate));
        ClinicName = ValidateRequiredString(command.ClinicName, ClinicNameMaxLength, nameof(command.ClinicName));
        ClinicUnit = ValidateRequiredString(command.ClinicUnit, ClinicUnitMaxLength, nameof(command.ClinicUnit));
        PatientName = ValidateRequiredString(effectivePatientName, PatientNameMaxLength, nameof(command.PatientName));
        Dni = ValidateRequiredString(command.Dni, DniMaxLength, nameof(command.Dni));

        if (command.Age is <= 0 or > 120)
            throw new ArgumentOutOfRangeException(nameof(command), command.Age, "Age must be between 1 and 120.");

        Diagnosis = ValidateRequiredString(command.Diagnosis, DiagnosisMaxLength, nameof(command.Diagnosis));
        ProsthesisCode = ValidateRequiredString(command.ProsthesisCode, ProsthesisCodeMaxLength,
            nameof(command.ProsthesisCode));
        EvaluationPeriod = ValidateRequiredString(command.EvaluationPeriod, EvaluationPeriodMaxLength,
            nameof(command.EvaluationPeriod));
        Summary = ValidateRequiredString(command.Summary, SummaryMaxLength, nameof(command.Summary));
        MedicalRecommendation = ValidateRequiredString(command.MedicalRecommendation, MedicalRecommendationMaxLength,
            nameof(command.MedicalRecommendation));
        TherapistName = ValidateRequiredString(command.TherapistName, TherapistNameMaxLength,
            nameof(command.TherapistName));
        TherapistRole = ValidateRequiredString(command.TherapistRole, TherapistRoleMaxLength,
            nameof(command.TherapistRole));

        ReportCode = command.ReportCode ?? throw new ArgumentNullException(nameof(command), "ReportCode is required.");
        PatientId = command.PatientId ?? throw new ArgumentNullException(nameof(command), "PatientId is required.");
        Age = command.Age;
        TherapistRegistry = command.TherapistRegistry ??
                            throw new ArgumentNullException(nameof(command), "TherapistRegistry is required.");

        Indicators = (command.Indicators ?? [])
            .Select(i => new ReportIndicator(i.Name, i.Value, i.NormalRange, i.Evaluation, i.Status))
            .ToList();
    }

    private static string ValidateRequiredString(string? value, int maxLength, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new ArgumentException($"{paramName} cannot exceed {maxLength} characters.", paramName);

        return trimmed;
    }
}
