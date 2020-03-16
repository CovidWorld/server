using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class SendNotificationToInactiveProfileCommand : IRequest
    {
        public Profile Profile { get; }
        public string Message { get; }

        public SendNotificationToInactiveProfileCommand(Profile profile, string message)
        {
            Profile = profile;
            Message = message;
        }
    }
}
