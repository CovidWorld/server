using System;
using System.Text.Json.Serialization;

namespace Sygic.Corona.Infrastructure.Services.Cosmos
{
    public class ProfileModel
    {
        [JsonPropertyName("ProfileId")] public uint ProfileId { get; set; }
        [JsonPropertyName("DeviceId")] public string DeviceId { get; set; }
        [JsonPropertyName("PhoneNumber")] public string PhoneNumber { get; set; }
        [JsonPropertyName("IsInQuarantine")] public bool IsInQuarantine { get; set; }
        [JsonPropertyName("LastPositionReportTime")] public DateTime? LastPositionReportTime { get; set; }
    }
}