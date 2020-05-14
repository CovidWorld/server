using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.NonceGenerating;
using System;
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

            if(instanceInfo.Platform != profile.ClientInfo.OperationSystem || instanceInfo.Application != profile.ClientInfo.Name)
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

            return pushNonce;
        }
    }
}
