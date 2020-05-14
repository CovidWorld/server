using MediatR;
using Sygic.Corona.Domain;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class GeneratePushNonceCommand : IRequest<PushNonce>
    {
        public GeneratePushNonceCommand(string deviceId, uint profileId, TimeSpan tokenExpiration)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            ProfileId = profileId;
            TokenExpiration = tokenExpiration;
        }

        public string DeviceId { get; }
        public uint ProfileId { get; }
        public TimeSpan TokenExpiration { get; set; }
    }
}
