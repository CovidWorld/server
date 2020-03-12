namespace Sygic.Corona.Contracts.Requests
{
    public class ConfirmInfectionRequest
    {
        public string DeviceId { get; set; }
        public uint ProfileId { get; set; }
        public string MfaToken { get; set; }
    }
}
