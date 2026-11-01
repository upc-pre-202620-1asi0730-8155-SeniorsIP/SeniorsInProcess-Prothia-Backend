using System.Net.Mime;
using Prothia.API.Analytics.Application.Services;
using Prothia.API.Analytics.Domain.Model.Queries;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Analytics.Interfaces.Rest.Resources;
using Prothia.API.Analytics.Interfaces.Rest.Transform;
using Prothia.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Prothia.API.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Reports")]
public class ReportsController(
    IClinicalReportCommandService clinicalReportCommandService,
    IClinicalReportQueryService clinicalReportQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<ReportsController> logger)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ClinicalReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateReport([FromBody] CreateClinicalReportResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateClinicalReportCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await clinicalReportCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateClinicalReportResultAssembler.ToActionResultFromCreateClinicalReportResult(
                result, this, localizer, nameof(GetReportById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating clinical report {ReportCode}",
                resource.ReportCode);
            return BadRequest(localizer["InvalidClinicalReportRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating clinical report {ReportCode}", resource.ReportCode);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingClinicalReport"].Value,
                statusCode: 500);
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClinicalReportResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetReports([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAllClinicalReportsQuery(patientId is null ? null : new PatientId(patientId.Value));
            var reports = await clinicalReportQueryService.Handle(query, cancellationToken);
            return Ok(reports.Select(ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while querying clinical reports for patient {PatientId}",
                patientId);
            return BadRequest(localizer["InvalidClinicalReportQuery"].Value);
        }
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClinicalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetReportById(int id, CancellationToken cancellationToken = default)
    {
        var report = await clinicalReportQueryService.Handle(new GetClinicalReportByIdQuery(id), cancellationToken);
        if (report is null) return NotFound();
        return Ok(ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }
}
