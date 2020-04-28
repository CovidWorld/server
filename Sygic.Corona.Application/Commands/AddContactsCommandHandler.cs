using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Commands
{
    public class AddContactsCommandHandler : AsyncRequestHandler<AddContactsCommand>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService convertService;

        public AddContactsCommandHandler(IRepository repository, IDateTimeConvertService convertService)
        {
            this.repository = repository;
            this.convertService = convertService;
        }

        protected override async Task Handle(AddContactsCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.SourceProfileId, request.SourceDeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            var t = convertService.UnixTimeStampToDateTime(request.Contacts.First().Timestamp);
            var f = t.Date;
            var contacts = request.Contacts
                .Select(x => new Contact(
                    profile.Id, profile.DeviceId, x.SeenProfileId, null,
                    convertService.UnixTimeStampToDateTime(x.Timestamp).Date, x.Duration))
                .ToList();

            profile.AddContact(contacts);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
