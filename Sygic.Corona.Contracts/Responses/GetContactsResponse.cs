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
        public uint ProfileId { get; set; }
        public string SourceDeviceId { get; set; }
        public uint SeenProfileId { get; set; }
        public int Timestamp { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
