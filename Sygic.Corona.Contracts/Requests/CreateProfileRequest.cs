namespace Sygic.Corona.Contracts.Requests
{
    public class CreateProfileRequest
    {
        public string DeviceId { get; set; }
        public string PushToken { get; set; }
        public string Locale { get; set; }
    }
}
