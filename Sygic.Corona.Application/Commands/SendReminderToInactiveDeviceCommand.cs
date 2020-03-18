using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class SendReminderToInactiveDeviceCommand : IRequest
    {
        private readonly List<Profile> profiles;
        public List<Profile> Profiles => profiles;

        protected SendReminderToInactiveDeviceCommand()
        {
            profiles = new List<Profile>();
        }

        public SendReminderToInactiveDeviceCommand(IEnumerable<Profile> profiles) : this()
        {
            this.profiles.AddRange(profiles);
        }
    }
}
