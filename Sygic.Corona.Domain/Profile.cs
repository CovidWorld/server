using System;
using System.Collections.Generic;

namespace Sygic.Corona.Domain
{
    public class Profile //Entity
    {
        public uint Id { get; private set; }
        public string DeviceId { get; private set; }
        public string PushToken { get; private set; }
        public string Locale { get; private set; }
        public Location Location { get; private set; }
        public string AuthToken { get; private set; }
        public bool ConfirmedInfection { get; private set; }

        private readonly List<Contact> contacts;
        public IReadOnlyCollection<Contact> Contacts => contacts;

        public Profile()
        {
            contacts = new List<Contact>();
        }
        public Profile(uint id, string deviceId, string pushToken, string locale, Location location, string authToken)
        {
            Id = id;
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            Location = location;
            AuthToken = authToken;
            ConfirmedInfection = false;
        }

        public void AddContact(uint seenProfileId, int timestamp, TimeSpan duration, Location location)
        {
            var contact = new Contact(Id, DeviceId, seenProfileId, timestamp, duration, location);
            contacts.Add(contact);
        }

        public void ConfirmInfection()
        {
            ConfirmedInfection = true;
        }
    }
}
