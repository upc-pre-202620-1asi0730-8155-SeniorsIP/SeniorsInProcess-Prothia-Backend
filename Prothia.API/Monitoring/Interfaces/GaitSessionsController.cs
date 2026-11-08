using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;
using Prothia.API.Monitoring.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;


namespace Prothia.API.Monitoring.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Gait Session Endpoints.")]
public class GaitSessionsController(
    IGaitSessionCommandService gaitSessionCommandService,
    IGaitSessionQueryService gaitSessionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Record Gait Session")]
    [EndpointDescription("Record a biomechanical gait session for a patient.")]
    [ProducesResponseType(typeof(GaitSessionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordGaitSession([FromBody] RecordGaitSessionResource resource,
        CancellationToken cancellationToken)
    {
        var command = RecordGaitSessionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await gaitSessionCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromGaitSessionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdSession => CreatedAtAction(nameof(GetGaitSessionById), new { id = createdSession.Id.Value },
                GaitSessionResourceFromEntityAssembler.ToResourceFromEntity(createdSession))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Gait Sessions")]
    [EndpointDescription("Get gait sessions optionally filtered by patientId.")]
    [ProducesResponseType(typeof(IEnumerable<GaitSessionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGaitSessions([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllGaitSessionsQuery(patientId is null ? null : new PatientId(patientId.Value));
        var sessions = await gaitSessionQueryService.Handle(query, cancellationToken);
        return Ok(sessions.Select(GaitSessionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Gait Session by Id")]
    [EndpointDescription("Get a gait session by its unique identifier.")]
    [ProducesResponseType(typeof(GaitSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGaitSessionById(int id, CancellationToken cancellationToken = default)
    {
        var session = await gaitSessionQueryService.Handle(new GetGaitSessionByIdQuery(id), cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromGetGaitSessionByIdResult(
            this,
            session,
            _errorLocalizer,
            _problemDetailsFactory,
            foundSession => Ok(GaitSessionResourceFromEntityAssembler.ToResourceFromEntity(foundSession))
        );
    }
}
