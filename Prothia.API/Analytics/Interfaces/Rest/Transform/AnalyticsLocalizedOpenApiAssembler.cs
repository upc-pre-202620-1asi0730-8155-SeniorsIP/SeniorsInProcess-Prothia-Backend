using Prothia.API.Analytics.Interfaces.Rest.Resources;
using Prothia.API.Shared.Interfaces.Asp.Configuration;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Prothia.API.Analytics.Interfaces.Rest.Transform;

public static class AnalyticsLocalizedOpenApiAssembler
{
    private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreateReport"] = LocalizedOpenApiTransformers.Operation("CreateReport",
                ("201", "Created"), ("400", "BadRequest"), ("404", "NotFound"), ("409", "Conflict"),
                ("500", "ServerError")),
            ["GetReports"] = LocalizedOpenApiTransformers.Operation("GetReports",
                ("200", "Success"), ("400", "BadRequest")),
            ["GetReportById"] = LocalizedOpenApiTransformers.Operation("GetReportById",
                ("200", "Success"), ("404", "NotFound"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(GenerateClinicalReportResource)] = "CreateClinicalReportSchema",
        [typeof(ClinicalReportResource)] = "ClinicalReportSchema",
        [typeof(ReportIndicatorResource)] = "ReportIndicatorSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties = BuildProperties();

    private static Dictionary<(Type, string), string> BuildProperties()
    {
        var properties = new Dictionary<(Type, string), string>();

        foreach (var type in new[] { typeof(ClinicalReportResource), typeof(GenerateClinicalReportResource) })
        {
            properties[(type, "reportCode")] = "ClinicalReportCode";
            properties[(type, "issueDate")] = "ClinicalReportIssueDate";
            properties[(type, "clinicName")] = "ClinicalReportClinicName";
            properties[(type, "clinicUnit")] = "ClinicalReportClinicUnit";
            properties[(type, "patientId")] = "ClinicalReportPatientId";
            properties[(type, "patientName")] = "ClinicalReportPatientName";
            properties[(type, "dni")] = "ClinicalReportDni";
            properties[(type, "age")] = "ClinicalReportAge";
            properties[(type, "diagnosis")] = "ClinicalReportDiagnosis";
            properties[(type, "prosthesisCode")] = "ClinicalReportProsthesisCode";
            properties[(type, "evaluationPeriod")] = "ClinicalReportEvaluationPeriod";
            properties[(type, "summary")] = "ClinicalReportSummary";
            properties[(type, "medicalRecommendation")] = "ClinicalReportMedicalRecommendation";
            properties[(type, "therapistName")] = "ClinicalReportTherapistName";
            properties[(type, "therapistRole")] = "ClinicalReportTherapistRole";
            properties[(type, "therapistRegistry")] = "ClinicalReportTherapistRegistry";
            properties[(type, "indicators")] = "ClinicalReportIndicators";
        }

        properties[(typeof(ClinicalReportResource), "id")] = "ClinicalReportId";

        properties[(typeof(ReportIndicatorResource), "name")] = "ReportIndicatorName";
        properties[(typeof(ReportIndicatorResource), "value")] = "ReportIndicatorValue";
        properties[(typeof(ReportIndicatorResource), "normalRange")] = "ReportIndicatorNormalRange";
        properties[(typeof(ReportIndicatorResource), "evaluation")] = "ReportIndicatorEvaluation";
        properties[(typeof(ReportIndicatorResource), "status")] = "ReportIndicatorStatus";

        return properties;
    }

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}
