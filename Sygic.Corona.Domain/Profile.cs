using System;
using System.Collections.Generic;

namespace Sygic.Corona.Domain
{
    public class Profile //Entity
    {
        public uint Id { get; private set; }
        public string DeviceId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PushToken { get; private set; }
        public string Locale { get; private set; }
        public AreaExit AreaExit { get; private set; }
        public string AuthToken { get; private set; }
        public bool ConfirmedInfection { get; private set; }
        public bool IsInQuarantine { get; private set; }
        public bool IsVerified { get; private set; }
        public DateTime? QuarantineBeginning { get; private set; }
        public DateTime? QuarantineEnd { get; private set; }
        public DateTime? LastPositionReportTime { get; private set; }
        public DateTime? LastInactivityNotificationSendTime { get; private set; }

        private readonly List<Contact> contacts;
        public IReadOnlyCollection<Contact> Contacts => contacts;

        private readonly List<Location> locations;
        public IReadOnlyCollection<Location> Locations => locations;

        public Profile()
        {
            contacts = new List<Contact>();
            locations = new List<Location>();
        }
        public Profile(uint id, string deviceId, string pushToken, string locale, Location location, string authToken, string phoneNumber) : this()
        {
            Id = id;
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            AuthToken = authToken;
            ConfirmedInfection = false;
            IsInQuarantine = false;
            IsVerified = false;
            PhoneNumber = phoneNumber;
        }

        public void AddContact(uint seenProfileId, int timestamp, TimeSpan duration, double? latitude, double? longitude, int? accuracy)
        {
            var contact = new Contact(Id, DeviceId, seenProfileId, timestamp, duration, latitude, longitude, accuracy);
            contacts.Add(contact);
        }

        public void AddLocations(IEnumerable<Location> locationsList)
        {
            locations.AddRange(locationsList);
            LastPositionReportTime = DateTime.UtcNow;
        }

        public void ConfirmInfection()
        {
            ConfirmedInfection = true;
        }

        public void AddAreaExit(AreaExit exit)
        {
            AreaExit = exit;
        }

        public void UpdatePushToken(string pushToken)
        {
            PushToken = pushToken;
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void BeginQuarantine(TimeSpan duration)
        {
            IsInQuarantine = true;
            QuarantineBeginning = DateTime.UtcNow;
            QuarantineEnd = QuarantineBeginning.Value.Add(duration);
        }

        public void SetInactivityNotificationSendTime(DateTime time)
        {
            LastInactivityNotificationSendTime = time;
        }

        public void Verify()
        {
            IsVerified = true;
        }
    }
}
