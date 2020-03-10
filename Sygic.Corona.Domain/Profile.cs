namespace Sygic.Corona.Domain
{
    public class Profile //Entity
    {
        public long Id { get; private set; }
        public string DeviceId { get; private set; }
        public string PushToken { get; private set; }
        public string Locale { get; private set; }
        public Location Location { get; private set; }
        public string AuthToken { get; private set; }
        public bool ConfirmedInfection { get; private set; }

        public Profile()
        {
            
        }
        public Profile(long id, string deviceId, string pushToken, string locale, Location location, string authToken)
        {
            Id = id;
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            Location = location;
            AuthToken = authToken;
            ConfirmedInfection = false;
        }
    }
}
