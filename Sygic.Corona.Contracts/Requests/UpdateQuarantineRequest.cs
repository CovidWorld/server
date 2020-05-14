using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Sygic.Corona.Contracts.Requests
{
    public class UpdateQuarantineRequest
    {
        [JsonProperty("vCovid19pass")]
        public string CovidPass { get; set; }
        [JsonProperty("dQuarantineStart")]
        public DateTime QuarantineStart { get; set; }
        [JsonProperty("dQuarantineEnd")]
        public DateTime QuarantineEnd { get; set; }
        [JsonProperty("vQuarantineAddressCountry")]
        public string QuarantineAddressCountry { get; set; }
        [JsonProperty("vQuarantineAddressCity")]
        public string QuarantineAddressCity { get; set; }
        [JsonProperty("vQuarantineAddressCityZipCode")]
        public string QuarantineAddressCityZipCode { get; set; }
        [JsonProperty("vQuarantineAddressStreetName")]
        public string QuarantineAddressStreetName { get; set; }
        [JsonProperty("vQuarantineAddressStreetNumber")]
        public string QuarantineAddressStreetNumber { get; set; }
        [JsonProperty("nQuarantineAddressLongitude")]
        public decimal QuarantineAddressLongitude { get; set; }
        [JsonProperty("nQuarantineAddressLatitude")]
        public decimal QuarantineAddressLatitude { get; set; }
        [JsonProperty("dBorderCrossedAt")]
        public DateTime BorderCrossedAt { get; set; }
    }
}