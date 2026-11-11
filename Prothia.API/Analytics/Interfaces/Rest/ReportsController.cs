using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Analytics.Application.CommandServices;
using Prothia.API.Analytics.Application.QueryServices;
using Prothia.API.Analytics.Domain.Model.Queries;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Analytics.Interfaces.Rest.Resources;
using Prothia.API.Analytics.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Reports Endpoints.")]
public class ReportsController(
    IClinicalReportCommandService clinicalReportCommandService,
    IClinicalReportQueryService clinicalReportQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Generate Clinical Reports")]
    [EndpointDescription("Issue a new clinical biomechanical report for a patient.")]
    [ProducesResponseType(typeof(ClinicalReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateReport([FromBody] GenerateClinicalReportResource resource,
        CancellationToken cancellationToken)
    {
        var command = GenerateClinicalReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await clinicalReportCommandService.Handle(command, cancellationToken);

        return AnalyticsActionResultAssembler.ToActionResultFromClinicalReportResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdReport => CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id.Value },
                ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity(createdReport))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Clinical Reports")]
    [EndpointDescription("Get clinical reports optionally filtered by patientId.")]
    [ProducesResponseType(typeof(IEnumerable<ClinicalReportResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetReports([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllClinicalReportsQuery(patientId is null ? null : new PatientId(patientId.Value));
        var reports = await clinicalReportQueryService.Handle(query, cancellationToken);
        return Ok(reports.Select(ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Clinical Reports by Id")]
    [EndpointDescription("Get a clinical report by its unique identifier.")]
    [ProducesResponseType(typeof(ClinicalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportById(int id, CancellationToken cancellationToken = default)
    {
        var report = await clinicalReportQueryService.Handle(new GetClinicalReportByIdQuery(id), cancellationToken);

        return AnalyticsActionResultAssembler.ToActionResultFromGetClinicalReportByIdResult(
            this,
            report,
            _errorLocalizer,
            _problemDetailsFactory,
            foundReport => Ok(ClinicalReportResourceFromEntityAssembler.ToResourceFromEntity(foundReport))
        );
    }
}
