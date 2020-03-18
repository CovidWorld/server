using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendReminderToInactiveDeviceCommandHandler : AsyncRequestHandler<SendReminderToInactiveDeviceCommand>
    {
        private readonly ICloudMessagingService messaging;

        public SendReminderToInactiveDeviceCommandHandler(ICloudMessagingService messaging)
        {
            this.messaging = messaging;
        }

        protected override async Task Handle(SendReminderToInactiveDeviceCommand request, CancellationToken cancellationToken)
        {
            foreach (var profile in request.Profiles)
            {
                if (!string.IsNullOrEmpty(profile.PushToken))
                {
                    var reminder = new Reminder { ContentAvailable = 1, Alert = "" };
                    await messaging.SendMessageToDevice(profile.PushToken, reminder, cancellationToken);
                }
            }
        }

        private class Reminder
        {
            [JsonProperty("content-available")]
            public byte ContentAvailable { get; set; }
            [JsonProperty("alert")]
            public string Alert { get; set; }
        }
    }
}
