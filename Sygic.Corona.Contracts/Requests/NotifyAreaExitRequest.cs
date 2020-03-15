using System;

namespace Sygic.Corona.Contracts.Requests
{
    public class NotifyAreaExitRequest
    {
        public uint ProfileId { get; set; }
        public string DeviceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Accuracy { get; set; }
        public int RecordTimestamp { get; set; }
    }
}
