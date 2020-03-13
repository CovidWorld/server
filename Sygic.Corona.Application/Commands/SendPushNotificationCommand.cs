using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class SendPushNotificationCommand : IRequest
    {
        public uint ProfileId { get; }
        public object Message { get; }

        public SendPushNotificationCommand(uint profileId, object message)
        {
            ProfileId = profileId;
            Message = message;
        }
    }
}
