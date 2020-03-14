using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class ConfirmInfectionCommandHandler : AsyncRequestHandler<ConfirmInfectionCommand>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public ConfirmInfectionCommandHandler(IRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }
        protected override async Task Handle(ConfirmInfectionCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }
            if (profile.AuthToken != request.MfaToken)
            {
                throw new DomainException("Wrong maf token.");
            }
            if (profile.AuthToken == request.MfaToken)
            {
                if (profile.ConfirmedInfection == false)
                {
                    profile.ConfirmInfection();
                    await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                    var query = new GetContactsQuery(profile.DeviceId, profile.Id);
                    var result = await mediator.Send(query, cancellationToken);
                    var groupedContacts = result.Contacts.GroupBy(x => x.ProfileId);
                    foreach (var contact in groupedContacts)
                    {
                        var firstContactFromGroup = contact.First();
                        var data = new { messageType = "CORONA_INFECTION_CONFIRMED" };
                        var command = new SendPushNotificationCommand(firstContactFromGroup.ProfileId, data);
                        await mediator.Send(command, cancellationToken);
                    }
                }
            }
        }
    }
}
