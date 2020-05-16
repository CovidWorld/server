using System;
using System.Collections.Generic;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetAllPresenceChecksResponse
    {
        public List<PresenceCheckResponse> Items { get; set; }
    }
    
    public class PresenceCheckResponse
    {
        public string CovidPass { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? LastHeartbeat { get; set; }
        public PresenceCheckStatus Status { get; set; }
    }
}