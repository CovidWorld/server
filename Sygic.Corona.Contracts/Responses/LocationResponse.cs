using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class LocationResponse
    {
        public Guid Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
