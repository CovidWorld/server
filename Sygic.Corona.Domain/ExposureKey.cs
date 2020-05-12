using System;

namespace Sygic.Corona.Domain
{
    public class ExposureKey //Entity
    {
        public Guid Id { get; private set; }
        public string Data { get; private set; }
        public int RollingStartNumber { get; private set; }
        public int RollingDuration { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime Expiration { get; private set; }

        public ExposureKey(string data, int rollingStartNumber, int rollingDuration, DateTime createdOn, DateTime expiration)
        {
            Data = data;
            RollingStartNumber = rollingStartNumber;
            RollingDuration = rollingDuration;
            CreatedOn = createdOn;
            Expiration = expiration;
        }

        public override string ToString()
        {
            return $"{Data},{RollingStartNumber},{RollingDuration}";
        }
    }
}
