using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetLastAlertForProfileQuery : IRequest<AlertResponse>
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }

        public GetLastAlertForProfileQuery(uint profileId, string deviceId)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
        }
    }
}
