namespace Sygic.Corona.Contracts.Requests
{
    public class GetProfilesByPhoneNumberRequest
    {
        public string SearchTherm { get; set; }
        public int Limit { get; set; }
    }
}
