using System;

namespace Sygic.Corona.Domain
{
    public class PushNonce //Entity
    {
        public string Id { get; private set; } // actualy push token is used like ID
        public string Body { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime ExpiredOn { get; private set; }

        public PushNonce(string id, string body, DateTime createdOn, DateTime expiredOn)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            CreatedOn = createdOn;
            ExpiredOn = expiredOn;
        }

        public void Update(string body, DateTime createdOn, DateTime expiredOn)
        {
            Body = body;
            CreatedOn = createdOn;
            ExpiredOn = expiredOn;
        }
    }
}
