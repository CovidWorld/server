using System;

namespace Sygic.Corona.Domain
{
    public class Contact // Entity
    {
        public Guid Id { get; private set; }
        public uint ProfileId { get; private set; }
        public string SourceDeviceId { get; private set; }
        public uint SeenProfileId { get; private set; }
        public int Timestamp { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Location Location { get; private set; }

        protected Contact()
        {
            Id = Guid.NewGuid();
        }

        public Contact(uint profileId, string sourceDeviceId, uint seenProfileId, int timestamp, TimeSpan duration, Location location): this()
        {
            ProfileId = profileId;
            SourceDeviceId = sourceDeviceId;
            SeenProfileId = seenProfileId;
            Timestamp = timestamp;
            Duration = duration;
            Location = location;
        }

    }
}
