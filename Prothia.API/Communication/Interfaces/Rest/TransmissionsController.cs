using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Communication.Application.CommandServices;
using Prothia.API.Communication.Application.QueryServices;
using Prothia.API.Communication.Domain.Model;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Communication.Interfaces.Rest.Resources;
using Prothia.API.Communication.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Communication.Interfaces.Rest;

/// <summary>
///     Clinical data transmissions' controller.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Transmissions")]
public class TransmissionsController(
    IClinicalTransmissionCommandService clinicalTransmissionCommandService,
    IClinicalTransmissionQueryService clinicalTransmissionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    ILogger<TransmissionsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a clinical data transmission to an orthoprosthetic workshop.
    /// </summary>
    [HttpPost]
    [EndpointSummary("Create Transmissions")]
    [EndpointDescription("Creates a clinical data transmission to an orthoprosthetic workshop.")]
    [ProducesResponseType(typeof(ClinicalTransmissionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransmission([FromBody] CreateClinicalTransmissionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateClinicalTransmissionCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await clinicalTransmissionCommandService.Handle(command, cancellationToken);

            return CommunicationActionResultAssembler.ToActionResultFromClinicalTransmissionResult(
                this,
                result,
                errorLocalizer,
                problemDetailsFactory,
                createdTransmission => CreatedAtAction(nameof(GetTransmissionById),
                    new { id = createdTransmission.Id.Value },
                    ClinicalTransmissionResourceFromEntityAssembler.ToResourceFromEntity(createdTransmission))
            );
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating clinical transmission for patient {PatientId}",
                resource.PatientId);
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                CommunicationError.InvalidCommunicationData,
                errorLocalizer[$"CommunicationError.{nameof(CommunicationError.InvalidCommunicationData)}"]
            );
        }
    }

    /// <summary>
    ///     Gets clinical data transmissions, optionally filtered by patient.
    /// </summary>
    [HttpGet]
    [EndpointSummary("Get Transmissions")]
    [EndpointDescription("Gets clinical data transmissions, optionally filtered by patient.")]
    [ProducesResponseType(typeof(IEnumerable<ClinicalTransmissionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTransmissions([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAllClinicalTransmissionsQuery(
                patientId is null ? null : new PatientId(patientId.Value));
            var transmissions = await clinicalTransmissionQueryService.Handle(query, cancellationToken);
            return Ok(transmissions.Select(ClinicalTransmissionResourceFromEntityAssembler.ToResourceFromEntity));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while querying clinical transmissions for patient {PatientId}",
                patientId);
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                CommunicationError.InvalidCommunicationData,
                errorLocalizer[$"CommunicationError.{nameof(CommunicationError.InvalidCommunicationData)}"]
            );
        }
    }

    /// <summary>
    ///     Gets a clinical data transmission by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [EndpointSummary("Get Transmissions by Id")]
    [EndpointDescription("Gets a clinical data transmission by its identifier.")]
    [ProducesResponseType(typeof(ClinicalTransmissionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransmissionById(int id, CancellationToken cancellationToken = default)
    {
        var transmission =
            await clinicalTransmissionQueryService.Handle(new GetClinicalTransmissionByIdQuery(id), cancellationToken);
        return CommunicationActionResultAssembler.ToActionResultFromGetClinicalTransmissionByIdResult(
            this,
            transmission,
            errorLocalizer,
            problemDetailsFactory,
            foundTransmission => Ok(ClinicalTransmissionResourceFromEntityAssembler.ToResourceFromEntity(foundTransmission))
        );
    }
}
