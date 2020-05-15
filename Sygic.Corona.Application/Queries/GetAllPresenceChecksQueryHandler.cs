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
    public class GetAllPresenceChecksQueryHandler : IRequestHandler<GetAllPresenceChecksQuery, GetAllPresenceChecksResponse>
    {
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public GetAllPresenceChecksQueryHandler(IRepository repository, CoronaContext context)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetAllPresenceChecksResponse> Handle(GetAllPresenceChecksQuery request, CancellationToken cancellationToken)
        {
            if (request.From > request.To)
            {
                throw new DomainException("Invalid time interval. Should be From <= To");
            }

            var presenceChecks = await context.PresenceChecks
                .Where(x => request.From.ToUniversalTime() <= x.CreatedOn && x.CreatedOn < request.To.ToUniversalTime())
                .Include(x => x.Profile)
                .OrderBy(x => x.CreatedOn)
                .AsNoTracking()
                .Select(x => new PresenceCheckResponse
                {
                    Id = x.Id,
                    CovidPass = x.Profile.CovidPass,
                    // DateTime retrieved from DB has DateTimeKind.Unspecified, which defaults to Local. We stored UTC and want to retrieve it as UTC.
                    CreatedOn = new DateTime(x.CreatedOn.Ticks, DateTimeKind.Utc) + request.Offset, // offset for NCZI (note that it possibly produces dates in the future)
                    UpdatedOn = new DateTime(x.UpdatedOn.Ticks, DateTimeKind.Utc) + request.Offset,
                    DeadLineCheck = new DateTime(x.DeadLineCheck.Ticks, DateTimeKind.Utc) + request.Offset,
                    Status = ToResponseStatus(x.Status)
                })
                .ToListAsync(cancellationToken);

            return new GetAllPresenceChecksResponse
            {
                Items = presenceChecks
            };
        }

        private static Sygic.Corona.Contracts.Requests.PresenceCheckStatus ToResponseStatus(PresenceCheckStatus domainPresenceCheckStatus)
        {
            return domainPresenceCheckStatus switch
            {
                PresenceCheckStatus.SUSPECTED => Sygic.Corona.Contracts.Requests.PresenceCheckStatus.SUSPECTED,
                PresenceCheckStatus.OK => Sygic.Corona.Contracts.Requests.PresenceCheckStatus.OK,
                PresenceCheckStatus.LEFT => Sygic.Corona.Contracts.Requests.PresenceCheckStatus.LEFT,
                _ => throw new ArgumentOutOfRangeException(nameof(domainPresenceCheckStatus), domainPresenceCheckStatus, null)
            };
        }
    }
}