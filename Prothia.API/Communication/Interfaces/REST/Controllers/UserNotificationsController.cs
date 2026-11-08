using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Communication.Application.Errors;
using Prothia.API.Communication.Application.Services;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Interfaces.REST.Resources;
using Prothia.API.Communication.Interfaces.REST.Transform;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Communication.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UserNotificationsController(
    ICommunicationNotificationCommandService commandService,
    ICommunicationNotificationQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PublishNotification([FromBody] PublishNotificationResource resource)
    {
        var command = PublishNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        
        if (result is Result<Communication.Domain.Model.Aggregates.UserNotification, EPublishNotificationError>.Failure failure)
            return BadRequest(new { Error = failure.Error.ToString() });

        var success = (Result<Communication.Domain.Model.Aggregates.UserNotification, EPublishNotificationError>.Success)result;
        var notificationResource = UserNotificationResourceFromEntityAssembler.ToResourceFromEntity(success.Value);
        
        return StatusCode(StatusCodes.Status201Created, notificationResource);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetNotificationsByUserId(Guid userId)
    {
        var query = new GetNotificationsByUserIdQuery(userId);
        var notifications = await queryService.Handle(query);
        var resources = notifications.Select(UserNotificationResourceFromEntityAssembler.ToResourceFromEntity);
        
        return Ok(resources);
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        var command = new MarkNotificationReadCommand(notificationId);
        var result = await commandService.Handle(command);
        
        if (result is Result<Communication.Domain.Model.Aggregates.UserNotification, EMarkNotificationReadError>.Failure failure)
            return NotFound(new { Error = failure.Error.ToString() });

        var success = (Result<Communication.Domain.Model.Aggregates.UserNotification, EMarkNotificationReadError>.Success)result;
        var notificationResource = UserNotificationResourceFromEntityAssembler.ToResourceFromEntity(success.Value);
        
        return Ok(notificationResource);
    }
}