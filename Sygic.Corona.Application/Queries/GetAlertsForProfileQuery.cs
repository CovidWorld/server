using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetAlertsForProfileQuery : IRequest<IEnumerable<AlertResponse>>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }

        public GetAlertsForProfileQuery(uint profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}
