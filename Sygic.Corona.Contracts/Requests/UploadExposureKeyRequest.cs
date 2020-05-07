using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sygic.Corona.Contracts.Requests
{
    public class UploadExposureKeyRequest
    {
        [JsonProperty("tekList")]
        public IEnumerable<ExposureKeyRequest> ExposureKeys { get; set; }
    }

    public class ExposureKeyRequest
    {
        [JsonProperty("tek")]
        public string TemporaryExposureKey { get; set; }
        public int RollingStartNumber { get; set; }
        public int RollingDuration { get; set; }
    }
}
