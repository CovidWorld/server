using MediatR;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class NotifyAreaExitCommand : IRequest
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }
        public int Severity { get; set; }
        public int RecordTimestamp { get; }

        public NotifyAreaExitCommand(uint profileId, string deviceId, int severity, int recordTimestamp)
        {
            ProfileId = profileId;
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            Severity = severity;
            RecordTimestamp = recordTimestamp;
        }
    }
}
