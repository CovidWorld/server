using Newtonsoft.Json;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public class Notification
    {
        [JsonProperty("priority")] public string Priority { get; set; }
        [JsonProperty("content-available")] public bool ContentAvailable { get; set; }
        [JsonProperty("data")] public NotificationData Data { get; set; }
        [JsonProperty("notification")] public NotificationContent NotificationContent { get; set; }
    }

    public class NotificationData
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class NotificationContent
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("body")] public string Body { get; set; }
        [JsonProperty("sound")] public string Sound { get; set; }
    }
}
