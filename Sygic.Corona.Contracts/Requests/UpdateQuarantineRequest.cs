using System;
using System.Text.Json.Serialization;

namespace Sygic.Corona.Contracts.Requests
{
    public class UpdateQuarantineRequest
    {
        [JsonPropertyName("vCovid19pass")]
        public string CovidPass { get; set; }
        [JsonPropertyName("dQuarantineStart")]
        public DateTime QuarantineStart { get; set; }
        [JsonPropertyName("dQuarantineEnd")]
        public DateTime QuarantineEnd { get; set; }
    }
}