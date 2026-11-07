using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Model.Queries;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Monitoring.Interfaces.Rest.Resources;
using Prothia.API.Monitoring.Interfaces.Rest.Transform;

namespace Prothia.API.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Threshold Profile Endpoints.")]
public class ThresholdProfilesController(
    IThresholdProfileCommandService thresholdProfileCommandService,
    IThresholdProfileQueryService thresholdProfileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Threshold Profile")]
    [EndpointDescription("Create a personalized clinical threshold profile for a patient.")]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateThresholdProfile([FromBody] CreateThresholdProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateThresholdProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await thresholdProfileCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromThresholdProfileResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdProfile => CreatedAtAction(nameof(GetThresholdProfileById), new { id = createdProfile.Id.Value },
                ThresholdProfileResourceFromEntityAssembler.ToResourceFromEntity(createdProfile))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Threshold Profiles")]
    [EndpointDescription("Get threshold profiles optionally filtered by patientId.")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdProfileResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetThresholdProfiles([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllThresholdProfilesQuery(patientId is null ? null : new PatientId(patientId.Value));
        var profiles = await thresholdProfileQueryService.Handle(query, cancellationToken);
        return Ok(profiles.Select(ThresholdProfileResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Threshold Profile by Id")]
    [EndpointDescription("Get a threshold profile by its unique identifier.")]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThresholdProfileById(int id, CancellationToken cancellationToken = default)
    {
        var profile = await thresholdProfileQueryService.Handle(new GetThresholdProfileByIdQuery(id),
            cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromGetThresholdProfileByIdResult(
            this,
            profile,
            _errorLocalizer,
            _problemDetailsFactory,
            foundProfile => Ok(ThresholdProfileResourceFromEntityAssembler.ToResourceFromEntity(foundProfile))
        );
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update Threshold Profile")]
    [EndpointDescription("Update an existing clinical threshold profile.")]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateThresholdProfile(int id,
        [FromBody] UpdateThresholdProfileResource resource, CancellationToken cancellationToken = default)
    {
        var command = UpdateThresholdProfileCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await thresholdProfileCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromThresholdProfileResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedProfile => Ok(ThresholdProfileResourceFromEntityAssembler.ToResourceFromEntity(updatedProfile))
        );
    }
}