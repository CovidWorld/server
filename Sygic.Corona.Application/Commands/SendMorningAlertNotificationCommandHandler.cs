using System;
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
                    await messagingService.SendMessageToDevice(request.ProfileInQuarantine.PushToken, new { type = "MORNING_QUARANTINE_ALERT", daysLeft = days }, cancellationToken);
                }
                
            }
        }
    }
}
