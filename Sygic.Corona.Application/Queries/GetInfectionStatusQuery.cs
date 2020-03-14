using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetInfectionStatusQuery : IRequest<GetInfectionStatusResponse>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }

        public GetInfectionStatusQuery(uint profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}
