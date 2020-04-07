using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class SendMfaTokenCommandHandler : IRequestHandler<SendMfaTokenCommand, SendMfaTokenResponse>
    {
        private readonly IRepository repository;
        private readonly ISmsMessagingService messagingService;
        private readonly ILogger log;

        public SendMfaTokenCommandHandler(IRepository repository, ISmsMessagingService messagingService, ILogger<SendMfaTokenCommandHandler> log)
        {
            this.repository = repository;
            this.messagingService = messagingService;
            this.log = log;
        }
        public async Task<SendMfaTokenResponse> Handle(SendMfaTokenCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
