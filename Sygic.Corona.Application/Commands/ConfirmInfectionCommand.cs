using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class ConfirmInfectionCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string MfaToken { get; set; }

        public ConfirmInfectionCommand(string deviceId, uint profileId, string mfaToken)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            MfaToken = mfaToken;
        }
    }
}
