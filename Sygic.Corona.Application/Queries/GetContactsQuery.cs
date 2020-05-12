using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetContactsQuery : IRequest<GetContactsResponse>
    {
        public string DeviceId { get; }
        public long ProfileId { get; }

        public GetContactsQuery(string deviceId, long profileId)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
        }
    }
}
