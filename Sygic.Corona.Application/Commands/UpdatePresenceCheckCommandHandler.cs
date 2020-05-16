using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Commands
{
    public class UpdatePresenceCheckCommandHandler : AsyncRequestHandler<UpdatePresenceCheckCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public UpdatePresenceCheckCommandHandler(IMediator mediator, IRepository repository, CoronaContext context)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override async Task Handle(UpdatePresenceCheckCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, request.CovidPass, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
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

            var now = DateTime.UtcNow;
            var pendingSuspectedPresenceCheck = await context.PresenceChecks
                .Where(x => x.ProfileId == profile.Id)
                .Where(x => x.Status == PresenceCheckStatus.SUSPECTED)
                .Where(x => x.CreatedOn <= now && now <= x.DeadLineCheck)
                .FirstOrDefaultAsync(cancellationToken);

            var newStatus = request.Status switch
            {
                Contracts.Requests.PresenceCheckStatus.OK => PresenceCheckStatus.OK,
                Contracts.Requests.PresenceCheckStatus.LEFT => PresenceCheckStatus.LEFT,
                _ => PresenceCheckStatus.SUSPECTED
            };

            pendingSuspectedPresenceCheck.UpdateStatus(newStatus);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}