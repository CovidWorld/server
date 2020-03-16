using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetDeviceWithContactsQuery : IRequest<GetDeviceWithContactsResponse>
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }

        public GetDeviceWithContactsQuery(uint profileId, string deviceId)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
        }
    }
}
