namespace Sygic.Corona.Contracts.Requests
{
    public class StoreNonceRequest : VerifiedRequestBase
    {
        public string CovidPass { get; set; }
    }
}