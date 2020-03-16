using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetDeviceWithLocQuery : IRequest<GetDeviceWithLocResponse>
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }

        public GetDeviceWithLocQuery(uint profileId, string deviceId)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
        }
    }
}
