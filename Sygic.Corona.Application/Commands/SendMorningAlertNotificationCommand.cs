using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class SendMorningAlertNotificationCommand : IRequest
    {
        public Profile ProfileInQuarantine { get; }
        public string Message { get; set; }

        public SendMorningAlertNotificationCommand(Profile profileInQuarantine, string message)
        {
            ProfileInQuarantine = profileInQuarantine;
            Message = message;
        }
    }
}
