using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Commands
{
    public class SendMfaTokenCommand : IRequest<SendMfaTokenResponse>
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string PhoneNumber { get; }

        public SendMfaTokenCommand(string deviceId, uint profileId, string phoneNumber)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            PhoneNumber = phoneNumber;
        }
    }
}
