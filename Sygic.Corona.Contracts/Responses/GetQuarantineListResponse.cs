using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetQuarantineListResponse
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public uint Id { get; set; }
        public string CovidPass { get; set; }
        public DateTime? CreatedOn { get; set; }
        public QuarantineResponse Quarantine { get; set; }
        public LastLocationResponse LastLocation { get; set; }
        
    }
    public class QuarantineResponse
    {
        public DateTime? QuarantineBeginning { get; set; }
        public DateTime? QuarantineEnd { get; set; }
        public QuarantineExitResponse QuarantineExit { get; set; }
    }
    public class QuarantineExitResponse
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
        public DateTime? AreaExitTime { get; set; }
    }

    public class LastLocationResponse
    {
        public DateTime? LastReportedTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
    }
}
