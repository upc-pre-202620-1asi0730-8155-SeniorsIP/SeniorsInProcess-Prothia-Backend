using Prothia.API.Analytics.Domain.Model.Commands;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Analytics.Interfaces.Rest.Resources;
using Prothia.API.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

namespace Prothia.API.Analytics.Interfaces.Rest.Transform;


public static class CreateClinicalReportCommandFromResourceAssembler
{
    public static CreateClinicalReportCommand ToCommandFromResource(CreateClinicalReportResource resource) =>
        new(
            new ReportCode(resource.ReportCode),
            resource.IssueDate,
            resource.ClinicName,
            resource.ClinicUnit,
            new PatientId(resource.PatientId),
            resource.PatientName ?? string.Empty,
            resource.Dni,
            resource.Age,
            resource.Diagnosis,
            resource.ProsthesisCode,
            resource.EvaluationPeriod,
            resource.Summary,
            resource.MedicalRecommendation,
            resource.TherapistName,
            resource.TherapistRole,
            new TherapistRegistry(resource.TherapistRegistry),
            (resource.Indicators ?? [])
            .Select(i => new ReportIndicatorItem(
                i.Name,
                i.Value,
                i.NormalRange,
                i.Evaluation,
                EnumConversions.Parse<IndicatorStatus>(i.Status, nameof(i.Status))))
            .ToList());
}