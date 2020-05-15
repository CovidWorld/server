using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Queries
{
    public class GetPresenceCheckQueryHandler : IRequestHandler<GetPresenceCheckQuery, GetPresenceCheckResponse>
    {
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public GetPresenceCheckQueryHandler(IRepository repository, CoronaContext context)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetPresenceCheckResponse> Handle(GetPresenceCheckQuery request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, request.CovidPass, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }
            
            var now = DateTime.UtcNow;
            var pendingSuspectedPresenceCheck = await context.PresenceChecks
                .Where(x => x.ProfileId == profile.Id)
                .Where(x => x.Status == PresenceCheckStatus.SUSPECTED)
                .Where(x => x.CreatedOn <= now && now <= x.DeadLineCheck)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return new GetPresenceCheckResponse
            {
                IsPresenceCheckPending = pendingSuspectedPresenceCheck != null
            };
        }
    }
}