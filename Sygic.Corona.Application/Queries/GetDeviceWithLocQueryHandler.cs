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
        public async Task<GetDeviceWithLocResponse> Handle(
            GetDeviceWithLocQuery q, 
            CancellationToken ct)
        {
            var profile = await repository.GetProfileAsyncNt(q.ProfileId, q.DeviceId, ct);

            if (profile == null)
            {
                return null;
            }
            
            var locations = await repository.GetLocationsForProfileNt(q.ProfileId, ct);
            
            var resp = new GetDeviceWithLocResponse
            {
                Id = profile.Id,
                DeviceId = profile.DeviceId,
                CovidPass = profile.CovidPass,
                QuarantineBeginning = profile.QuarantineBeginning,
                QuarantineEnd = profile.QuarantineEnd,
                Locations = locations.Select(l => new LocationResponse
                {
                    Id = l.Id,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Accuracy = l.Accuracy,
                    CreatedOn = l.CreatedOn
                }).ToList()
            };

            return resp;
        }
    }
}
