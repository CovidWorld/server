using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendMorningAlertNotificationCommandHandler : AsyncRequestHandler<SendMorningAlertNotificationCommand>
    {
        private readonly ICloudMessagingService messagingService;

        public SendMorningAlertNotificationCommandHandler(ICloudMessagingService messagingService)
        {
            this.messagingService = messagingService;
        }

        protected override async Task Handle(SendMorningAlertNotificationCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.ProfileInQuarantine.PushToken))
            {
                var daysLeft = (request.ProfileInQuarantine.QuarantineEnd - DateTime.UtcNow);
                if (daysLeft.HasValue)
                {
                    string days = daysLeft.Value.Days.ToString();
                    string messageText = !string.IsNullOrEmpty(request.Message)
                        ? string.Format(request.Message, days) 
                        : string.Empty;

                    var message = new Notification
                    {
                        Priority = "high",
                        Data = new NotificationData
                        {
                            Type = "MORNING_QUARANTINE_ALERT"
                        },
                        NotificationContent = new NotificationContent
                        {
                            Title = "",
                            Body = messageText,
                            Sound = "default"
                        },
                        ContentAvailable = true
                    };
                    await messagingService.SendMessageToDevice(request.ProfileInQuarantine.PushToken, message, cancellationToken);
                }
                
            }
        }

        private class Notification
        {
            [JsonProperty("priority")] public string Priority { get; set; }
            [JsonProperty("content-available")] public bool ContentAvailable { get; set; }
            [JsonProperty("data")] public NotificationData Data { get; set; }
            [JsonProperty("notification")] public NotificationContent NotificationContent { get; set; }
        }

        private class NotificationData
        {
            [JsonProperty("type")] public string Type { get; set; }
        }

        private class NotificationContent
        {
            [JsonProperty("title")] public string Title { get; set; }
            [JsonProperty("body")] public string Body { get; set; }
            [JsonProperty("sound")] public string Sound { get; set; }
        }
    }
}
