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

            var checks = await context.PresenceChecks.AsNoTracking()
                    .Join(context.Profiles,
                        check => check.ProfileId,
                        profile => profile.Id,
                        (check, profile) => new
                        {
                            Check = check,
                            profile.CovidPass,
                            profile.LastPositionReportTime
                        })
                    .OrderBy(x => x.Check.CreatedOn)
                    .ToListAsync(cancellationToken);

            var presenceChecks = checks.Select(x =>
                new PresenceCheckResponse
                {
                    CovidPass = x.CovidPass,
                    Status = ToResponseStatus(x.Check.Status),
                        // DateTime retrieved from DB has DateTimeKind.Unspecified, which defaults to Local. We stored UTC and want to retrieve it as UTC.
                        CreatedOn = new DateTime(x.Check.CreatedOn.Ticks, DateTimeKind.Utc) + request.Offset, // offset for NCZI (note that it possibly produces dates in the future)
                        UpdatedOn = new DateTime(x.Check.UpdatedOn.Ticks, DateTimeKind.Utc) + request.Offset,
                    LastHeartbeat = x.LastPositionReportTime.HasValue
                            ? new DateTime(x.LastPositionReportTime.Value.Ticks, DateTimeKind.Utc)
                            : default(DateTime?)
                })
                .ToList();

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