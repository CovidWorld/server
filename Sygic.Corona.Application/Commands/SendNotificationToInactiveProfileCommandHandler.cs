using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendNotificationToInactiveProfileCommandHandler : AsyncRequestHandler<SendNotificationToInactiveProfileCommand>
    {
        private readonly ISmsMessagingService messagingService;
        private readonly ILogger<SendNotificationToInactiveProfileCommandHandler> log;
        private readonly IRepository repository;

        public SendNotificationToInactiveProfileCommandHandler(ISmsMessagingService messagingService,
            ILogger<SendNotificationToInactiveProfileCommandHandler> log, IRepository repository)
        {
            this.messagingService = messagingService;
            this.log = log;
            this.repository = repository;
        }
        protected override async Task Handle(SendNotificationToInactiveProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.Profile.IsInQuarantine)
            {
                if (!string.IsNullOrEmpty(request.Profile.PhoneNumber))
                {
                    if (request.Profile.LastInactivityNotificationSendTime == null || request.Profile.LastInactivityNotificationSendTime.Value.AddDays(1) < DateTime.UtcNow)
                    {
                        try
                        {
                            await messagingService.SendMessageAsync(request.Message, request.Profile.PhoneNumber, cancellationToken);

                            log.LogInformation("Inactivity SMS message send", new { phoneNumber = request.Profile.PhoneNumber, type = "INACTIVITY_SMS_SEND"});

                            request.Profile.SetInactivityNotificationSendTime(DateTime.UtcNow);
                            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
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
}
