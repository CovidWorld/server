using System;
using System.Collections.Generic;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetContactsResponse
    {
        public List<ContactResponse> Contacts { get; set; }

        public GetContactsResponse()
        {
            Contacts = new List<ContactResponse>();
        }
    }

    public class ContactResponse
    {
        public long ProfileId { get; set; }
        public string SourceDeviceId { get; set; }
        public long SeenProfileId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public TimeSpan? Duration { get; set; }
        public ContactLocationResponse Location { get; set; }
    }

    public class ContactLocationResponse 
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Accuracy { get; set; }
    }
}
