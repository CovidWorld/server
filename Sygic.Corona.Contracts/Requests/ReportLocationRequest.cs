using System.Collections.Generic;

namespace Sygic.Corona.Contracts.Requests
{
    public class ReportLocationRequest
    {
        public uint ProfileId { get; set; }
        public string DeviceId { get; set; }

        public List<LocationRequest> Locations { get; set; }
    }

    public class LocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Accuracy { get; set; }
        public int RecordTimestamp { get; set; }
    }
}
