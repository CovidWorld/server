using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class StartQuarantineCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string MfaToken { get; }
        public TimeSpan QuarantineDuration { get; }

        public StartQuarantineCommand(string deviceId, uint profileId, string mfaToken, TimeSpan quarantineDuration)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            MfaToken = mfaToken;
            QuarantineDuration = quarantineDuration;
        }
    }
}
