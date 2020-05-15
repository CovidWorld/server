using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetQuarantineResponse
    {
        public string CovidPass { get; set; }
        public DateTime QuarantineStart { get; set; }
        public DateTime QuarantineEnd { get; set; }
        public AddressResponse Address { get; set; }
        public DateTime BorderCrossedAt { get; set; }
    }

    public class AddressResponse
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
    }
}