using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetProfilesInQuarantineQueryHandler : IRequestHandler<GetProfilesInQuarantineQuery, IEnumerable<Profile>>
    {
        private readonly IRepository repository;

        public GetProfilesInQuarantineQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Profile>> Handle(GetProfilesInQuarantineQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetRawProfilesInQuarantineAsync(cancellationToken);
        }
    }
}
