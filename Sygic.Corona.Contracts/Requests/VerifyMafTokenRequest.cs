namespace Sygic.Corona.Contracts.Requests
{
    public class VerifyMafTokenRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public string MfaToken { get; set; }
    }
}
