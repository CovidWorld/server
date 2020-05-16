using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class UpdatePresenceCheckCommand : IRequest
    {
        public long ProfileId { get; }
        public string DeviceId { get; set; }
        public string CovidPass { get; }
        public PresenceCheckStatus Status { get; }
        public string Nonce { get; }

        public UpdatePresenceCheckCommand(long profileId, string deviceId, string covidPass, PresenceCheckStatus status, string nonce)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
            CovidPass = covidPass;
            Status = status;
            Nonce = nonce;
        }
    }
}