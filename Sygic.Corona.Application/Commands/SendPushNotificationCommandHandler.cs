using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendPushNotificationCommandHandler : AsyncRequestHandler<SendPushNotificationCommand>
    {
        private readonly IRepository repository;
        private readonly ICloudMessagingService messagingService;

        public SendPushNotificationCommandHandler(IRepository repository, ICloudMessagingService messagingService)
        {
            this.repository = repository;
            this.messagingService = messagingService;
        }

        protected override async Task Handle(SendPushNotificationCommand request, CancellationToken cancellationToken)
        {
            string token = await repository.GetProfilePushTokenAsync(request.ProfileId, cancellationToken);
            var result = await messagingService.SendMessageToDevice(token, request.Message, cancellationToken);
        }
    }
}
