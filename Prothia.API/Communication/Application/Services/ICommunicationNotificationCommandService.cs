using Prothia.API.Communication.Application.Errors;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Communication.Application.Services;

public interface ICommunicationNotificationCommandService
{
    Task<Result<Message, ESendMessageError>> Handle(SendMessageCommand command);
    Task<Result<UserNotification, EPublishNotificationError>> Handle(PublishNotificationCommand command);
    Task<Result<UserNotification, EMarkNotificationReadError>> Handle(MarkNotificationReadCommand command);
}