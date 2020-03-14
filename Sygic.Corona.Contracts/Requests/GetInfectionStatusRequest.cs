namespace Sygic.Corona.Contracts.Requests
{
    public class GetInfectionStatusRequest
    {
        public uint ProfileId { get; set; }
        public string DeviceId { get; set; }
    }
}
