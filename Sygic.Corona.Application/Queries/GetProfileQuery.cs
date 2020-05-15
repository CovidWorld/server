using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineQuery : IRequest<GetQuarantineResponse>
    {
        public int ProfileId { get; }
        public string DeviceId { get; }

        public GetQuarantineQuery(int profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}