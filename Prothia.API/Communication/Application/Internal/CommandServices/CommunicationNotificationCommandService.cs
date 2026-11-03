using Prothia.API.Communication.Application.Errors;
using Prothia.API.Communication.Application.Services;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.CommandServices;

public class CommunicationNotificationCommandService(
    IMessageRepository messageRepository,
    IUserNotificationRepository userNotificationRepository,
    IUnitOfWork unitOfWork) : ICommunicationNotificationCommandService
{
    public async Task<Result<Message, ESendMessageError>> Handle(SendMessageCommand command)
    {
        try
        {
            var message = new Message(command.SenderUserId, command.RecipientUserId, command.Body);
            await messageRepository.AddAsync(message);
            await unitOfWork.CompleteAsync();
            return new Result<Message, ESendMessageError>.Success(message);
        }
        catch (Exception)
        {
            return new Result<Message, ESendMessageError>.Failure(ESendMessageError.UnexpectedError);
        }
    }

    public async Task<Result<UserNotification, EPublishNotificationError>> Handle(PublishNotificationCommand command)
    {
        try
        {
            var notification = new UserNotification(command.UserId, command.Type, command.ReferenceId);
            await userNotificationRepository.AddAsync(notification);
            await unitOfWork.CompleteAsync();
            return new Result<UserNotification, EPublishNotificationError>.Success(notification);
        }
        catch (Exception)
        {
            return new Result<UserNotification, EPublishNotificationError>.Failure(EPublishNotificationError.UnexpectedError);
        }
    }

    public async Task<Result<UserNotification, EMarkNotificationReadError>> Handle(MarkNotificationReadCommand command)
    {
        try
        {
            var notification = await userNotificationRepository.FindByIdAsync(command.NotificationId);
            
            if (notification is null)
                return new Result<UserNotification, EMarkNotificationReadError>.Failure(EMarkNotificationReadError.NotificationNotFound);

            notification.MarkRead();
            userNotificationRepository.Update(notification);
            await unitOfWork.CompleteAsync();
            
            return new Result<UserNotification, EMarkNotificationReadError>.Success(notification);
        }
        catch (Exception)
        {
            return new Result<UserNotification, EMarkNotificationReadError>.Failure(EMarkNotificationReadError.UnexpectedError);
        }
    }
}