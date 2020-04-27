using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class AddContactsCommandHandler : AsyncRequestHandler<AddContactsCommand>
    {
        private readonly IRepository repository;

        public AddContactsCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(AddContactsCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.SourceProfileId, request.SourceDeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            var contacts = request.Contacts
                .Select(x => new Contact(profile.Id, profile.DeviceId, x.SeenProfileId, x.Timestamp, x.Duration))
                .ToList();

            profile.AddContact(contacts);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
