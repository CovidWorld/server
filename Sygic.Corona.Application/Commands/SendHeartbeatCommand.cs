using MediatR;
using Sygic.Corona.Domain;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class SendHeartbeatCommand : IRequest
    {
        public SendHeartbeatCommand(string deviceId, uint profileId, string covidPass)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            ProfileId = profileId;
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
        }

        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string CovidPass { get; }
    }
}
