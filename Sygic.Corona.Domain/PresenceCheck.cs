using System;

namespace Sygic.Corona.Domain
{
    public class PresenceCheck // Entity
    {
        public Guid Id { get; private set; }
        public long ProfileId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        public DateTime DeadLineCheck { get; private set; }
        public PresenceCheckStatus Status { get; private set; }

        public PresenceCheck(long profileId, DateTime createdOn, DateTime deadLineCheck, PresenceCheckStatus status)
        {
            ProfileId = profileId;
            CreatedOn = createdOn.ToUniversalTime();
            UpdatedOn = createdOn.ToUniversalTime();
            DeadLineCheck = deadLineCheck.ToUniversalTime();
            Status = status;
        }

        public void UpdateStatus(PresenceCheckStatus status)
        {
            Status = status;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
