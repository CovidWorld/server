using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.NonceGenerating;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Commands
{
    public class GeneratePushNonceCommandHandler : IRequestHandler<GeneratePushNonceCommand, PushNonce>
    {
        private readonly CoronaContext context;
        private readonly IRepository repository;
        private readonly INonceGenerator nonceGenerator;
        private readonly IInstanceIdService instanceIdService;
        private readonly IMediator mediator;

        public GeneratePushNonceCommandHandler(CoronaContext context, IRepository repository, INonceGenerator nonceGenerator, IInstanceIdService instanceIdService, IMediator mediator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.nonceGenerator = nonceGenerator ?? throw new ArgumentNullException(nameof(nonceGenerator));
            this.instanceIdService = instanceIdService ?? throw new ArgumentNullException(nameof(instanceIdService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PushNonce> Handle(GeneratePushNonceCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            if (string.IsNullOrEmpty(profile.PushToken))
            {
                throw new DomainException("Push token is required please update profile");
            }

            var instanceInfo = await instanceIdService.GetInstanceInfoAsync(profile.PushToken, cancellationToken);

            if (instanceInfo == null)
            {
                throw new DomainException("Push token is invalid");
            }

            if(instanceInfo.Platform != profile.ClientInfo.OperationSystem || instanceInfo.Application != profile.ClientInfo.Integrator)
            {
                throw new DomainException("Push token is invalid or from wrong platform");
            }

            var pushNonce = await context.PushNonces.SingleOrDefaultAsync(x => x.Id == profile.PushToken, cancellationToken);

            if (pushNonce != null)
            {
                pushNonce.Update(nonceGenerator.Generate(), DateTime.UtcNow, DateTime.UtcNow.Add(request.TokenExpiration));
            }
            else
            {
                pushNonce = new PushNonce(profile.PushToken, nonceGenerator.Generate(), DateTime.UtcNow, DateTime.UtcNow.Add(request.TokenExpiration));
                await context.PushNonces.AddAsync(pushNonce, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            var message = new Notification
            {
                Data = new Dictionary<string, object>
                {
                    { "type", "PUSH_NONCE" },
                    { "Nonce", pushNonce.Body }
                },
                Priority = "high",
                ContentAvailable = true
            };
            var sendNonceCommand = new SendPushNotificationCommand(profile.Id, message);
            await mediator.Send(sendNonceCommand, cancellationToken);

            return pushNonce;
        }
    }
}
