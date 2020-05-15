namespace Sygic.Corona.Contracts.Requests
{
    public class NotifyAreaExitRequest
    {
        public uint ProfileId { get; set; }
        public string DeviceId { get; set; }
        public int Severity { get; set; }
        public int RecordTimestamp { get; set; }
    }
}
