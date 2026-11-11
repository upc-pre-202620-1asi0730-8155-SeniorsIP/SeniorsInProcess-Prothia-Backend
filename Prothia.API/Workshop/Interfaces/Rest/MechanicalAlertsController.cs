using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Commands;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Interfaces.Rest.Resources;
using Prothia.API.Workshop.Interfaces.Rest.Transform;

namespace Prothia.API.Workshop.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Mechanical Alert Endpoints.")]
public class MechanicalAlertsController(
    IMechanicalAlertCommandService mechanicalAlertCommandService,
    IMechanicalAlertQueryService mechanicalAlertQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Mechanical Alert")]
    [EndpointDescription("Raise a mechanical alert for a prosthesis in the workshop.")]
    [ProducesResponseType(typeof(MechanicalAlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateMechanicalAlert([FromBody] CreateMechanicalAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateMechanicalAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await mechanicalAlertCommandService.Handle(command, cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromMechanicalAlertResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdAlert => CreatedAtAction(nameof(GetMechanicalAlertById), new { id = createdAlert.Id.Value },
                MechanicalAlertResourceFromEntityAssembler.ToResourceFromEntity(createdAlert))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Mechanical Alerts")]
    [EndpointDescription("Get mechanical alerts optionally filtered by prosthesisId or status.")]
    [ProducesResponseType(typeof(IEnumerable<MechanicalAlertResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMechanicalAlerts([FromQuery] int? prosthesisId = null,
        [FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        if (prosthesisId is <= 0)
            throw new ArgumentOutOfRangeException(nameof(prosthesisId), prosthesisId,
                "ProsthesisId must be greater than zero.");

        var query = new GetAllMechanicalAlertsQuery(
            prosthesisId,
            status is null ? null : EnumConversions.Parse<MechanicalAlertStatus>(status, nameof(status)));
        var alerts = await mechanicalAlertQueryService.Handle(query, cancellationToken);
        return Ok(alerts.Select(MechanicalAlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Mechanical Alert by Id")]
    [EndpointDescription("Get a mechanical alert by its unique identifier.")]
    [ProducesResponseType(typeof(MechanicalAlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMechanicalAlertById(int id, CancellationToken cancellationToken = default)
    {
        var alert = await mechanicalAlertQueryService.Handle(new GetMechanicalAlertByIdQuery(id), cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromGetMechanicalAlertByIdResult(
            this,
            alert,
            _errorLocalizer,
            _problemDetailsFactory,
            foundAlert => Ok(MechanicalAlertResourceFromEntityAssembler.ToResourceFromEntity(foundAlert))
        );
    }

    [HttpPut("{id:int}/resolve")]
    [EndpointSummary("Resolve Mechanical Alert")]
    [EndpointDescription("Resolve a pending mechanical alert.")]
    [ProducesResponseType(typeof(MechanicalAlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResolveMechanicalAlert(int id, CancellationToken cancellationToken = default)
    {
        var result = await mechanicalAlertCommandService.Handle(new SolveMechanicalAlertCommand(id),
            cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromMechanicalAlertResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            resolvedAlert => Ok(MechanicalAlertResourceFromEntityAssembler.ToResourceFromEntity(resolvedAlert))
        );
    }
}
