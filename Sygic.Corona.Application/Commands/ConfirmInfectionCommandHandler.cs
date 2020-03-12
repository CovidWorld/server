using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class ConfirmInfectionCommandHandler : AsyncRequestHandler<ConfirmInfectionCommand>
    {
        private readonly IRepository repository;

        public ConfirmInfectionCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }
        protected override async Task Handle(ConfirmInfectionCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile.AuthToken == request.MfaToken)
            {
                profile.ConfirmInfection();
                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
