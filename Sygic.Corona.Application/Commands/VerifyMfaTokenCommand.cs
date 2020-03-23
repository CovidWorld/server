using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class VerifyMfaTokenCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string MfaToken { get; }

        public VerifyMfaTokenCommand(string deviceId, uint profileId, string mfaToken)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            MfaToken = mfaToken;
        }
    }
}
