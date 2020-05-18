using MediatR;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class SendHeartbeatCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string CovidPass { get; }
        public string Nonce { get; set; }

        public SendHeartbeatCommand(string deviceId, uint profileId, string covidPass, string nonce)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            ProfileId = profileId;
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
            Nonce = nonce;
        }     
    }
}
