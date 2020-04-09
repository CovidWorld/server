using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineListQueryHandler : IRequestHandler<GetQuarantineListQuery, IEnumerable<GetQuarantineListResponse>>
    {
        private readonly IRepository repository;

        public GetQuarantineListQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<GetQuarantineListResponse>> Handle(GetQuarantineListQuery request, CancellationToken cancellationToken)
        {
            var profiles = await repository.GetProfilesInQuarantineAsync(request.From, cancellationToken);
            return profiles;
        }
    }
}
