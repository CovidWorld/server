using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
                    string messageText = 
                        $"Váš zostávajúci počet dní v karanténe: {days}. Ďakujeme že dodržiavate domácu karanténu. Správajme sa zodpovedne voči blízkym a nášmu okoliu. MV SR";
                    var message = new Notification
                    {
                        Priority = "high",
                        Data = new Dictionary<string, object>
                        {
                            { "type", "MORNING_QUARANTINE_ALERT" }
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
    }
}
