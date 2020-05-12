using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class SendPushNotificationCommand : IRequest
    {
        public long ProfileId { get; }
        public object Message { get; }

        public SendPushNotificationCommand(long profileId, object message)
        {
            ProfileId = profileId;
            Message = message;
        }
    }
}
