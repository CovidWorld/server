using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineQuery : IRequest<GetQuarantineResponse>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }

        public GetQuarantineQuery(uint profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}