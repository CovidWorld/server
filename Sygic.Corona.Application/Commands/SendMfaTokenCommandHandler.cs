using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendMfaTokenCommandHandler : IRequestHandler<SendMfaTokenCommand, SendMfaTokenResponse>
    {
        private readonly IRepository repository;
        private readonly ISmsMessagingService messagingService;

        public SendMfaTokenCommandHandler(IRepository repository, ISmsMessagingService messagingService)
        {
            this.repository = repository;
            this.messagingService = messagingService;
        }
        public async Task<SendMfaTokenResponse> Handle(SendMfaTokenCommand request, CancellationToken cancellationToken)
        {
            string token = await repository.GetProfileMfaTokenAsync(request.ProfileId, request.DeviceId, cancellationToken);
            await messagingService.SendMessageAsync(token, request.PhoneNumber, cancellationToken);

            return new SendMfaTokenResponse();
        }
    }
}
