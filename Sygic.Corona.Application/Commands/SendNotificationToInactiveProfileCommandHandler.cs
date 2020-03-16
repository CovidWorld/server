using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendNotificationToInactiveProfileCommandHandler : AsyncRequestHandler<SendNotificationToInactiveProfileCommand>
    {
        private readonly ISmsMessagingService messagingService;
        private readonly ILogger<SendNotificationToInactiveProfileCommandHandler> log;

        public SendNotificationToInactiveProfileCommandHandler(ISmsMessagingService messagingService, ILogger<SendNotificationToInactiveProfileCommandHandler> log)
        {
            this.messagingService = messagingService;
            this.log = log;
        }
        protected override async Task Handle(SendNotificationToInactiveProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.Profile.IsInQuarantine)
            {
                if (!string.IsNullOrEmpty(request.Profile.PhoneNumber))
                {
                    try
                    {
                        await messagingService.SendMessageAsync(request.Message, request.Profile.PhoneNumber, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        log.LogError(new EventId((int)request.Profile.Id), ex.Message);
                    }
                }
            }
        }
    }
}
