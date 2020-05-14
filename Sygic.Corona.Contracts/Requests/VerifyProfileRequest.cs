namespace Sygic.Corona.Contracts.Requests
{
    public class VerifyProfileRequest
    {
        public string DeviceId { get; set; }
        public long ProfileId { get; set; }
        public string CovidPass { get; set; }
        public string Nonce { get; set; }
    }
}
