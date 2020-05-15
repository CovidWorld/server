using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetPresenceCheckQuery : IRequest<GetPresenceCheckResponse>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }
        public string CovidPass { get; }

        public GetPresenceCheckQuery(uint profileId, string deviceId, string covidPass)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
            CovidPass = covidPass;
        }
    }
}