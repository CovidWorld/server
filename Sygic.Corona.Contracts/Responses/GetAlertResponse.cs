using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class AlertResponse
    {
        public Guid Id { get; set; }
        public long Created { get; set; }
        public string Content { get; set; }
    }
}