using System;

namespace Sygic.Corona.Domain
{
    public class AreaExit
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double Accuracy { get; private set; }
        public DateTime RecordDateUtc { get; private set; }

        public AreaExit(double latitude, double longitude, double accuracy, DateTime recordDateUtc)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
            RecordDateUtc = recordDateUtc;
        }
    }
}
