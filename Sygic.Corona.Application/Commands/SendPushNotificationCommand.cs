using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class SendPushNotificationCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public object Message { get; }

        public SendPushNotificationCommand(string deviceId, uint profileId, object message)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            Message = message;
        }
    }
}
