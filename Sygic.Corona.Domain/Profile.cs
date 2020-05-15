using System;
using System.Collections.Generic;

namespace Sygic.Corona.Domain
{
    public class Profile //Entity
    {
        public long Id { get; private set; }
        public string DeviceId { get; private set; }
        public string PushToken { get; private set; }
        public string Locale { get; private set; }
        public ClientInfo ClientInfo { get; set; }
        public Address QuarantineAddress { get; private set; }
        public string AuthToken { get; private set; }
        public bool ConfirmedInfection { get; private set; }
        public bool IsInQuarantine { get; private set; }
        public bool IsVerified { get; private set; }
        public string MedicalId { get; private set; }
        public string CovidPass { get; private set; }
        public string PublicKey { get; private set; }
        public DateTime? CreatedOn { get; private set; }
        public DateTime? BorderCrossedAt { get; private set; }
        public DateTime? QuarantineBeginning { get; private set; }
        public DateTime? QuarantineEnd { get; private set; }
        public DateTime? LastPositionReportTime { get; private set; }
        public DateTime? LastInactivityNotificationSendTime { get; private set; }

        private readonly List<Contact> contacts;
        public IReadOnlyCollection<Contact> Contacts => contacts;

        private readonly List<Location> locations;
        public IReadOnlyCollection<Location> Locations => locations;

        private readonly List<Alert> alerts;
        public IReadOnlyCollection<Alert> Alerts => alerts;

        private readonly List<AreaExit> areaExits;
        public IReadOnlyCollection<AreaExit> AreaExits => areaExits;

        private readonly List<PresenceCheck> presenceChecks;
        public IReadOnlyCollection<PresenceCheck> PresenceChecks => presenceChecks;

        public Profile()
        {
            contacts = new List<Contact>();
            locations = new List<Location>();
            alerts = new List<Alert>();
            areaExits = new List<AreaExit>();
            presenceChecks = new List<PresenceCheck>();
            CreatedOn = DateTime.UtcNow;
        }
        public Profile(string deviceId, string pushToken, string locale, string authToken) : this()
        {
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            AuthToken = authToken;
            ConfirmedInfection = false;
            IsInQuarantine = false;
            IsVerified = false;
        }

        public void AddContact(IEnumerable<Contact> contactsList)
        {
            contacts.AddRange(contactsList);
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
            areaExits.Add(exit);
        }

        public void UpdatePushToken(string pushToken)
        {
            PushToken = pushToken;
        }
        
        public void BeginQuarantine(TimeSpan duration)
        {
            IsInQuarantine = true;
            QuarantineBeginning = DateTime.UtcNow;
            QuarantineEnd = QuarantineBeginning.Value.Add(duration);
        }

        public void BeginQuarantine(DateTime from, DateTime to)
        {
            IsInQuarantine = true;
            QuarantineBeginning = from.ToUniversalTime();
            QuarantineEnd = to.ToUniversalTime();
        }

        public void UpdateQuarantine(DateTime start, DateTime end, DateTime borderCrossedAt, Address quarantineAddress)
        {
            IsInQuarantine = true;
            QuarantineBeginning = start.ToUniversalTime();
            QuarantineEnd = end.ToUniversalTime();
            BorderCrossedAt = borderCrossedAt.ToUniversalTime();
            QuarantineAddress = quarantineAddress;
        }

        public void SetInactivityNotificationSendTime(DateTime time)
        {
            LastInactivityNotificationSendTime = time.ToUniversalTime();
        }

        public void Verify()
        {
            IsVerified = true;
        }

        public void ChangeMedicalId(string medicalId)
        {
            MedicalId = medicalId;
        }

        public void AssignCovidPass(string covidPass)
        {
            CovidPass = covidPass;
        }
        
        public void AssignPublicKey(string publicKey)
        {
            PublicKey = publicKey;
        }

        public void AddAlert(Alert alert)
        {
            alerts.Add(alert);
        }

        public void AddClientInfo(ClientInfo clientInfo)
        {
            ClientInfo = clientInfo;
        }

        public void UpdateLastPositionReportTime(DateTime lastPositionReportTime)
        {
            LastPositionReportTime = lastPositionReportTime.ToUniversalTime();
        }

        public void AddPresenceCheck(PresenceCheck presenceCheck)
        {
            presenceChecks.Add(presenceCheck);
        }
    }
}
