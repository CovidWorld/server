using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetInfectionStatusQueryHandler : IRequestHandler<GetInfectionStatusQuery, GetInfectionStatusResponse>
    {
        private readonly IRepository repository;

        public GetInfectionStatusQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<GetInfectionStatusResponse> Handle(GetInfectionStatusQuery request, CancellationToken cancellationToken)
        {
            bool status = await repository.GetProfileInfectionStatusAsync(request.ProfileId, request.DeviceId, cancellationToken);

            return new GetInfectionStatusResponse { IsInfected = status };
        }
    }
}
