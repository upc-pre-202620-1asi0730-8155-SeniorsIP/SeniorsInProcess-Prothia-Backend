using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Commands;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;
using Prothia.API.Monitoring.Interfaces.Rest.Transform;
using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;


namespace Prothia.API.Monitoring.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Alert Endpoints.")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Alert")]
    [EndpointDescription("Raise a biomechanical alert for a patient.")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAlert([FromBody] CreateAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromAlertResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdAlert => CreatedAtAction(nameof(GetAlertById), new { id = createdAlert.Id.Value },
                AlertResourceFromEntityAssembler.ToResourceFromEntity(createdAlert))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Alerts")]
    [EndpointDescription("Get biomechanical alerts optionally filtered by patientId or status.")]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAlerts([FromQuery] int? patientId = null,
        [FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        var query = new GetAllAlertsQuery(
            patientId is null ? null : new PatientId(patientId.Value),
            status is null ? null : EnumConversions.Parse<EAlertStatus>(status, nameof(status)));
        var alerts = await alertQueryService.Handle(query, cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Alert by Id")]
    [EndpointDescription("Get a biomechanical alert by its unique identifier.")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAlertById(int id, CancellationToken cancellationToken = default)
    {
        var alert = await alertQueryService.Handle(new GetAlertByIdQuery(id), cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromGetAlertByIdResult(
            this,
            alert,
            _errorLocalizer,
            _problemDetailsFactory,
            foundAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(foundAlert))
        );
    }

    [HttpPut("{id:int}/resolve")]
    [EndpointSummary("Resolve Alert")]
    [EndpointDescription("Mark a pending biomechanical alert as resolved.")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResolveAlert(int id, CancellationToken cancellationToken = default)
    {
        var result = await alertCommandService.Handle(new ResolveAlertCommand(id), cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromAlertResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            resolvedAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(resolvedAlert))
        );
    }
}
