using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public class CloudMessagingResponse
    {
        [JsonProperty("multicast_id")]
        public string MulticastId { get; set; }

        [JsonProperty("canonical_ids")]
        public int CanonicalIds { get; set; }

        /// <summary>
        /// Success count
        /// </summary>
        public int Success { get; set; }

        /// <summary>
        /// Failure count
        /// </summary>
        public int Failure { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public List<CloudMessagingResult> Results { get; set; }

        /// <summary>
        /// Returns value indicating notification sent success or failure
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return Success > 0 && Failure == 0;
        }
    }

    public class CloudMessagingResult
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        public string Error { get; set; }
    }
}
