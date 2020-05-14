using System;

namespace Sygic.Corona.Domain
{
    public class AreaExit //Entity
    {
        public Guid Id { get; private set; }
        public long ProfileId { get; private set; }
        public int Severity { get; private set; }
        public DateTime RecordDateUtc { get; private set; }

        public AreaExit(long profileId, int severity, DateTime recordDateUtc)
        {
            ProfileId = profileId;
            Severity = severity;
            RecordDateUtc = recordDateUtc;
        }
    }
}
