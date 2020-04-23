namespace Sygic.Corona.Contracts.Requests
{
    public class CreateAlertRequest
    {
        public string CovidPass { get; set; }
        public string Content { get; set; }
        public bool? WithPushNotification { get; set; }
        public string PushSubject { get; set; }
        public string PushBody { get; set; }
    }
}
