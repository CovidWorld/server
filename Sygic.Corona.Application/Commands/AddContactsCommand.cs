using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class AddContactsCommand : IRequest
    {
        public string SourceDeviceId { get; }
        public uint SourceProfileId { get; }

        private readonly List<CreateConnectionRequest> contacts;
        public List<CreateConnectionRequest> Contacts => contacts;

        public AddContactsCommand()
        {
            contacts = new List<CreateConnectionRequest>();
        }
        public AddContactsCommand(string sourceDeviceId, uint sourceProfileId , IEnumerable<CreateConnectionRequest> contacts) : this()
        {
            SourceDeviceId = sourceDeviceId;
            SourceProfileId = sourceProfileId;
            this.contacts.AddRange(contacts);
        }
    }
}
