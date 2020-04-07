using System;

namespace Sygic.Corona.Contracts.Requests
{
    public class StartQuarantineRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public string CovidPass { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
