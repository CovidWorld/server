namespace Sygic.Corona.Contracts.Requests
{
    public class HeartbeatRequest : VerifiedRequestBase
    {
        public string CovidPass { get; set; }
        public string Nonce { get; set; }
    }
}
