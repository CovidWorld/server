using System;

namespace Sygic.Corona.Contracts.Requests
{
    public class StartQuarantineRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public string MfaToken { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
