using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Interfaces.Rest.Resources;
using Prothia.API.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

namespace Prothia.API.Analytics.Interfaces.Rest.Transform;

public static class ClinicalReportResourceFromEntityAssembler
{
    public static ClinicalReportResource ToResourceFromEntity(ClinicalReport entity) =>
        new(
            entity.Id,
            entity.ReportCode.Value,
            entity.IssueDate,
            entity.ClinicName,
            entity.ClinicUnit,
            entity.PatientId.Value,
            entity.PatientName,
            entity.Dni,
            entity.Age,
            entity.Diagnosis,
            entity.ProsthesisCode,
            entity.EvaluationPeriod,
            entity.Summary,
            entity.MedicalRecommendation,
            entity.TherapistName,
            entity.TherapistRole,
            entity.TherapistRegistry.Value,
            entity.Indicators.Select(i => new ReportIndicatorResource(
                i.Name,
                i.Value,
                i.NormalRange,
                i.Evaluation,
                i.Status.ToCamelCaseString())).ToList());
}