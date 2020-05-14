namespace Sygic.Corona.Contracts.Requests
{
    public class HeartbeatRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public string CovidPass { get; set; }
    }
}
