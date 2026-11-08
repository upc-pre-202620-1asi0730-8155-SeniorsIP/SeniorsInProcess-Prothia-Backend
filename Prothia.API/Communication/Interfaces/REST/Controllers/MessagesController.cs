using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Communication.Application.Errors;
using Prothia.API.Communication.Application.Services;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Interfaces.REST.Resources;
using Prothia.API.Communication.Interfaces.REST.Transform;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Communication.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class MessagesController(
    ICommunicationNotificationCommandService commandService,
    ICommunicationNotificationQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageResource resource)
    {
        var command = SendMessageCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        
        if (result is Result<Communication.Domain.Model.Aggregates.Message, ESendMessageError>.Failure failure)
            return BadRequest(new { Error = failure.Error.ToString() });

        var success = (Result<Communication.Domain.Model.Aggregates.Message, ESendMessageError>.Success)result;
        var messageResource = MessageResourceFromEntityAssembler.ToResourceFromEntity(success.Value);
        
        return StatusCode(StatusCodes.Status201Created, messageResource);
    }

    [HttpGet("{userId}/conversation/{otherUserId}")]
    public async Task<IActionResult> GetConversation(Guid userId, Guid otherUserId)
    {
        var query = new GetConversationQuery(userId, otherUserId);
        var messages = await queryService.Handle(query);
        var resources = messages.Select(MessageResourceFromEntityAssembler.ToResourceFromEntity);
        
        return Ok(resources);
    }
}