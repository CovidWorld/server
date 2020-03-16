using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetDeviceWithContactsQueryHandler : IRequestHandler<GetDeviceWithContactsQuery, GetDeviceWithContactsResponse>
    {
        private readonly IRepository repository;

        public GetDeviceWithContactsQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<GetDeviceWithContactsResponse> Handle(
            GetDeviceWithContactsQuery q, 
            CancellationToken ct)
        {
            var profile = await repository.GetProfileAsyncNt(q.ProfileId, q.DeviceId, ct);

            if (profile == null)
            {
                return null;
            }
            
            var contacts = await repository.GetContactsForProfileAsyncNt(q.ProfileId, ct);
            
            var resp = new GetDeviceWithContactsResponse
            {
                Id = profile.Id,
                DeviceId = profile.DeviceId,
                PhoneNumber = profile.PhoneNumber,
                QuarantineBeginning = profile.QuarantineBeginning,
                QuarantineEnd = profile.QuarantineEnd,
                
                Contacts = contacts.Select(x => new ContactResponse
                {
                    ProfileId = x.ProfileId,
                    SourceDeviceId = x.SourceDeviceId,
                    SeenProfileId = x.SeenProfileId,
                    Duration = x.Duration,
                    Timestamp = x.Timestamp
                }).ToList()
            };

            return resp;
        }
    }
}
