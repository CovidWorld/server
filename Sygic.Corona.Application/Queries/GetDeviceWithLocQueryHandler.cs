using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetDeviceWithLocQueryHandler : IRequestHandler<GetDeviceWithLocQuery, GetDeviceWithLocResponse>
    {
        private readonly IRepository repository;

        public GetDeviceWithLocQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public Task<GetDeviceWithLocResponse> Handle(
            GetDeviceWithLocQuery q, 
            CancellationToken ct)
        {
            return repository.GetDeviceWithLocHistoryAsync(q.ProfileId, q.DeviceId, ct);
        }
    }
}
