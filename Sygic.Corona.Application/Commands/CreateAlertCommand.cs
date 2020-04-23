using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class CreateAlertCommand : IRequest
    {
        public string CovidPass { get; }
        public string Content { get; }
        public bool? WithPushNotification { get; }
        public string PushSubject { get; }
        public string PushBody { get; }

        public CreateAlertCommand(string covidPass, string content, bool? withPushNotification, string pushSubject, string pushBody)
        {
            CovidPass = covidPass;
            Content = content;
            WithPushNotification = withPushNotification;
            PushSubject = pushSubject;
            PushBody = pushBody;
        }
    }
}
