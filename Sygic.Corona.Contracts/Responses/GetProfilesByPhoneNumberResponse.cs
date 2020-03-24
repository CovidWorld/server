using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetProfilesByPhoneNumberResponse
    {
        public string PhoneNumber { get; set; }
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public DateTime? LastPositionReportTime { get; set; }
        public bool IsInQuarantine { get; set; }
    }
}
