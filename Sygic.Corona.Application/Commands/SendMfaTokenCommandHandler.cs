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
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            if (string.IsNullOrEmpty(profile.PhoneNumber))
            {
                throw new DomainException("Phone number not found on profile. Update phone number.");
            }

            try
            {
                string message = $"Vas verifikacny kod: {profile.AuthToken}. Covid-19 App";
                await messagingService.SendMessageAsync(message, profile.PhoneNumber, cancellationToken);
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                log.LogError(new EventId((int) profile.Id), ex, "sms API exception");
                throw new DomainException("Phone number is iw wrong format or region is unsupported.");
            }
            catch (Exception ex)
            {
                log.LogError(new EventId((int)profile.Id), ex, "sms API exception");
                throw;
            }
            

            return new SendMfaTokenResponse();
        }
    }
}
