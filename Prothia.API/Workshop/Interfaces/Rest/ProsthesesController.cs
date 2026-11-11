using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Interfaces.Rest.Resources;
using Prothia.API.Workshop.Interfaces.Rest.Transform;

namespace Prothia.API.Workshop.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Prostheses Endpoints.")]
public class ProsthesesController(
    IProsthesisCommandService prosthesisCommandService,
    IProsthesisQueryService prosthesisQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Prostheses")]
    [EndpointDescription("Register a prosthesis in the workshop inventory.")]
    [ProducesResponseType(typeof(ProsthesisResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProsthesis([FromBody] CreateProsthesisResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateProsthesisCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await prosthesisCommandService.Handle(command, cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromProsthesisResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdProsthesis => CreatedAtAction(nameof(GetProsthesisById),
                new { id = createdProsthesis.Id.Value },
                ProsthesisResourceFromEntityAssembler.ToResourceFromEntity(createdProsthesis))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Prostheses")]
    [EndpointDescription("Get all prostheses in the workshop inventory.")]
    [ProducesResponseType(typeof(IEnumerable<ProsthesisResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProstheses(CancellationToken cancellationToken = default)
    {
        var prostheses = await prosthesisQueryService.Handle(new GetAllProsthesesQuery(), cancellationToken);
        return Ok(prostheses.Select(ProsthesisResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Prostheses by Id")]
    [EndpointDescription("Get a prosthesis by its unique identifier.")]
    [ProducesResponseType(typeof(ProsthesisResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProsthesisById(int id, CancellationToken cancellationToken = default)
    {
        var prosthesis = await prosthesisQueryService.Handle(new GetProsthesisByIdQuery(id), cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromGetProsthesisByIdResult(
            this,
            prosthesis,
            _errorLocalizer,
            _problemDetailsFactory,
            foundProsthesis => Ok(ProsthesisResourceFromEntityAssembler.ToResourceFromEntity(foundProsthesis))
        );
    }

    [HttpPut("{id:int}/associate")]
    [EndpointSummary("Associate Prostheses")]
    [EndpointDescription("Associate a prosthesis in inventory with a patient and clinic.")]
    [ProducesResponseType(typeof(ProsthesisResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssociateProsthesis(int id,
        [FromBody] AssociateProsthesisResource resource,
        CancellationToken cancellationToken = default)
    {
        var result = await prosthesisCommandService.AssociateAsync(id, resource.PatientName, resource.ClinicName,
            cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromProsthesisResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedProsthesis => Ok(ProsthesisResourceFromEntityAssembler.ToResourceFromEntity(updatedProsthesis))
        );
    }
}

public record AssociateProsthesisResource(string? PatientName, string? ClinicName);
