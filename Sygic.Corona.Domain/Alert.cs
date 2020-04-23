using System;

namespace Sygic.Corona.Domain
{
    public class Alert //Entity
    {
        public Guid Id { get; private set; }
        public string DeviceId { get; private set; }
        public uint ProfileId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string Content { get; private set; }

        public Alert(string deviceId, uint profileId, DateTime createdOn, string content)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            CreatedOn = createdOn;
            Content = content;
        }
    }
}
