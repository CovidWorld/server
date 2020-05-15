using System;

namespace Sygic.Corona.Contracts.Requests
{
    public class GetAllPresenceChecksRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}