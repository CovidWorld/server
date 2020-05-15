namespace Sygic.Corona.Contracts.Requests
{
    public class NotifyAreaExitRequest : VerifiedRequestBase
    {
        public int Severity { get; set; }
        public int RecordTimestamp { get; set; }
    }
}
