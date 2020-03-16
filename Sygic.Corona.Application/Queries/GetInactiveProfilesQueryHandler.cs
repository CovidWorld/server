using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetInactiveProfilesQueryHandler : IRequestHandler<GetInactiveProfilesQuery, IEnumerable<Profile>>
    {
        private readonly IRepository repository;

        public GetInactiveProfilesQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Profile>> Handle(GetInactiveProfilesQuery request, CancellationToken cancellationToken)
        {
            var inactiveFromTime = request.CheckDate.Add(-request.CheckInterval);
            var profiles = await repository.GetInactiveUsersInQuarantineAsync(inactiveFromTime, cancellationToken);
            return profiles;
        }
    }
}
