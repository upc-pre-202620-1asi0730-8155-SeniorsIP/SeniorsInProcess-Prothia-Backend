using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Communication.Application.CommandServices;
using Prothia.API.Communication.Application.QueryServices;
using Prothia.API.Communication.Domain.Model;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Interfaces.Rest.Resources;
using Prothia.API.Communication.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Communication.Interfaces.Rest;

/// <summary>
///     Clinical messages' controller.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Messages")]
public class MessagesController(
    IClinicalMessageCommandService clinicalMessageCommandService,
    IClinicalMessageQueryService clinicalMessageQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    ILogger<MessagesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a clinical message in the interdisciplinary thread.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClinicalMessageResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMessage([FromBody] CreateClinicalMessageResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateClinicalMessageCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await clinicalMessageCommandService.Handle(command, cancellationToken);

            return CommunicationActionResultAssembler.ToActionResultFromClinicalMessageResult(
                this,
                result,
                errorLocalizer,
                problemDetailsFactory,
                createdMessage => CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id.Value },
                    ClinicalMessageResourceFromEntityAssembler.ToResourceFromEntity(createdMessage))
            );
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating clinical message from {SenderName}",
                resource.SenderName);
            return problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                CommunicationError.InvalidCommunicationData,
                errorLocalizer[$"CommunicationError.{nameof(CommunicationError.InvalidCommunicationData)}"]
            );
        }
    }

    /// <summary>
    ///     Gets all clinical messages.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClinicalMessageResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessages(CancellationToken cancellationToken = default)
    {
        var messages = await clinicalMessageQueryService.Handle(new GetAllClinicalMessagesQuery(), cancellationToken);
        return Ok(messages.Select(ClinicalMessageResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets a clinical message by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClinicalMessageResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageById(int id, CancellationToken cancellationToken = default)
    {
        var message = await clinicalMessageQueryService.Handle(new GetClinicalMessageByIdQuery(id), cancellationToken);
        return CommunicationActionResultAssembler.ToActionResultFromGetClinicalMessageByIdResult(
            this,
            message,
            errorLocalizer,
            problemDetailsFactory,
            foundMessage => Ok(ClinicalMessageResourceFromEntityAssembler.ToResourceFromEntity(foundMessage))
        );
    }
}
