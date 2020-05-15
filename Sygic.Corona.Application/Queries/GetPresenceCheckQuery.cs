using System;
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
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
        }
    }
}