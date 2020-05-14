namespace Sygic.Corona.Domain
{
    public class Address //Value object
    {
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        public string CountryCode { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string StreetName { get; private set; }
        public string StreetNumber { get; private set; }

        public Address(decimal latitude, decimal longitude, string countryCode, string city, string zipCode, string streetName, string streetNumber)
        {
            Latitude = latitude;
            Longitude = longitude;
            CountryCode = countryCode;
            City = city;
            ZipCode = zipCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
        }
    }
}