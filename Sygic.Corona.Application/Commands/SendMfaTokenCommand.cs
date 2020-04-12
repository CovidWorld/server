using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Commands
{
    public class SendMfaTokenCommand : IRequest<SendMfaTokenResponse>
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string Message { get; set; }

        public SendMfaTokenCommand(string deviceId, uint profileId, string message)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            Message = message;
        }
    }
}
