namespace Sygic.Corona.Contracts.Requests
{
    public class GetPushNonceRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
    }
}
