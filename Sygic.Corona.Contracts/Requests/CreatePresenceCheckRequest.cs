using Newtonsoft.Json;

namespace Sygic.Corona.Contracts.Requests
{
    public class CreatePresenceCheckRequest
    {
        [JsonProperty("vCovid19Pass")]
        public string CovidPass { get; set; }
    }
}