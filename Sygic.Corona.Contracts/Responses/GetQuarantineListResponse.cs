using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetQuarantineListResponse
    {
        public uint Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime? QuarantineBeginning { get; set; }
        public DateTime? QuarantineEnd { get; set; }
        public DateTime? LastPositionReportTime { get; set; }
        public AreaExitResponse AreaExit { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
    }

    public class AreaExitResponse
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
        public DateTime? RecordDate { get; set; }
    }
}
