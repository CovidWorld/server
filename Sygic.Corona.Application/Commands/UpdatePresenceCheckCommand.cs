using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class UpdatePresenceCheckCommand : IRequest
    {
        public uint ProfileId { get; }
        public string CovidPass { get; }
        public PresenceCheckStatus Status { get; }
        public string Nonce { get; }

        public UpdatePresenceCheckCommand(uint profileId, string deviceId, string covidPass, PresenceCheckStatus status, string nonce)
        {
            ProfileId = profileId;
            CovidPass = covidPass;
            Status = status;
            Nonce = nonce;
        }
    }
}