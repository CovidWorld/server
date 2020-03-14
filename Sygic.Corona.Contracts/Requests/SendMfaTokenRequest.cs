namespace Sygic.Corona.Contracts.Requests
{
    public class SendMfaTokenRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
    }
}
