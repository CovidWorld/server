using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineStatusQuery : IRequest<GetQuarantineStatusResponse>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }

        public GetQuarantineStatusQuery(uint profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}
