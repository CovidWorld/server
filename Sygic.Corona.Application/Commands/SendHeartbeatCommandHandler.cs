using MediatR;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Commands
{
    public class SendHeartbeatCommandHandler : AsyncRequestHandler<SendHeartbeatCommand>
    {
        private readonly CoronaContext context;
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public SendHeartbeatCommandHandler(CoronaContext context, IRepository repository, IMediator mediator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(SendHeartbeatCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, request.CovidPass, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            if (!profile.ActiveQuarantine(DateTime.UtcNow))
            {
                throw new DomainException("Profile not in quarantine");
            }

            var nonceQuery = new RetrieveNonceQuery(profile.CovidPass);
            var nonceCache = await mediator.Send(nonceQuery, cancellationToken);

            if (nonceCache == null)
            {
                throw new DomainException("Invalid nonce");
            }

            if (nonceCache.Nonce != request.Nonce)
            {
                throw new DomainException("Invalid nonce");
            }

            profile.UpdateLastPositionReportTime(DateTime.UtcNow);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}