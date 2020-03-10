namespace Sygic.Corona.Domain
{
    public class Location //Value object
    {
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }
        public int? Accuracy { get; private set; }

        public Location(double? latitude, double? longitude, int? accuracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }
    }
}
