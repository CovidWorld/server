namespace Sygic.Corona.Contracts.Requests
{
    public abstract class VerifiedRequestBase
    {
        public uint ProfileId { get; set; }
        public string DeviceId { get; set; }
    }
}