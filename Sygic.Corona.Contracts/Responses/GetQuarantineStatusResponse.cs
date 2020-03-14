using System;

namespace Sygic.Corona.Contracts.Responses
{
    public class GetQuarantineStatusResponse
    {
        public bool IsInQuarantine { get; set; }
        public DateTime? QuarantineBeginning { get; set; }
        public DateTime? QuarantineEnd { get; set; }
    }
}
