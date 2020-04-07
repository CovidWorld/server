using System;
using System.Collections.Generic;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetDeviceWithLocResponse
    {
        public uint Id { get; set; }
        public string DeviceId { get; set; }
        public string CovidPass { get; set; }
        public DateTime? QuarantineBeginning { get; set; }
        public DateTime? QuarantineEnd { get; set; }
        public IList<LocationResponse> Locations { get; set; }
    }
}