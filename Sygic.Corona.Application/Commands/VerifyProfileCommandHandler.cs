using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Services.AndroidAttestation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Commands
{
    public class VerifyProfileCommandHandler : AsyncRequestHandler<VerifyProfileCommand>
    {
        private readonly CoronaContext context;
        private readonly IRepository repository;
        private readonly IAndroidAttestation androidAttestation;

        public VerifyProfileCommandHandler(CoronaContext context, IRepository repository, IAndroidAttestation androidAttestation)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.androidAttestation = androidAttestation ?? throw new ArgumentNullException(nameof(androidAttestation));
        }

        protected override async Task Handle(VerifyProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            var now = DateTime.UtcNow;
            var nonce = await context.PushNonces.SingleOrDefaultAsync(x => x.Id == profile.PushToken 
                    && x.ExpiredOn > DateTime.UtcNow, cancellationToken);
            if (nonce?.Body != request.Nonce)
            {
                throw new DomainException("Nonce not found or expired");
            }

            if (profile.ClientInfo.OperationSystem == "anr")
            {
                var attestation = androidAttestation.ParseAndVerify(request.SignedAttestationStatement);

                if (attestation == null)
                {
                    throw new DomainException("Device isn't attested");
                }
            }

            profile.AssignCovidPass(request.CovidPass);
            profile.Verify();

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
