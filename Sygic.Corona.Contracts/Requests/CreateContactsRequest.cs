using System;
using System.Collections.Generic;

namespace Sygic.Corona.Contracts.Requests
{
    public class CreateContactsRequest
    {
        public string SourceDeviceId { get; set; }
        public uint SourceProfileId { get; set; }
        public List<CreateConnectionRequest> Connections { get; set; }
    }

    public class CreateConnectionRequest
    {
        public int Timestamp { get; set; }
        public uint SeenProfileId { get; set; }
        public TimeSpan? Duration { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
    }
}
