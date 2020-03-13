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

            foreach (var contactRequest in request.Contacts)
            {
                var location = new Location(contactRequest.Latitude, contactRequest.Longitude, contactRequest.Accuracy);
                var contact = new Contact(profile.Id, profile.DeviceId, contactRequest.SeenProfileId,
                    contactRequest.Timestamp, contactRequest.Duration, location);

                //profile.AddContact(contact.SeenDeviceId, contact.Timestamp, contact.Duration, contact.Location);
                await repository.CreateContactAsync(contact, cancellationToken);
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
