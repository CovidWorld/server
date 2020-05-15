using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Queries
{
    public class GetPresenceCheckQueryHandler : IRequestHandler<GetPresenceCheckQuery, GetPresenceCheckResponse>
    {
        private readonly IRepository repository;
        private readonly CoronaContext coronaContext;

        public GetPresenceCheckQueryHandler(IRepository repository, CoronaContext coronaContext)
        {
            this.repository = repository;
            this.coronaContext = coronaContext;
        }

        public Task<GetPresenceCheckResponse> Handle(GetPresenceCheckQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}