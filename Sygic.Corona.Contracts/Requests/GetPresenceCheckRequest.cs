namespace Sygic.Corona.Contracts.Requests
{
    public class GetPresenceCheckRequest : VerifiedRequestBase
    {
        public string CovidPass { get; set; }
    }
}