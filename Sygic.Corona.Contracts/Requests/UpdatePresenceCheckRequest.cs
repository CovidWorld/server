namespace Sygic.Corona.Contracts.Requests
{
    public class UpdatePresenceCheckRequest : VerifiedRequestBase
    {
        public string CovidPass { get; set; }
        public PresenceCheckStatus Status { get; set; }
        public string Nonce { get; set; }
    }

    public enum PresenceCheckStatus
    {
        OK = 1,
        LEFT
    }
}