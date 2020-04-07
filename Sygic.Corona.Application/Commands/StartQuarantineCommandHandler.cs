using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class StartQuarantineCommandHandler : AsyncRequestHandler<StartQuarantineCommand>
    {
        private readonly IRepository repository;

        public StartQuarantineCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(StartQuarantineCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }
            
            if (profile.IsInQuarantine == false)
            {
                profile.BeginQuarantine(request.StartDate, request.EndDate);
                profile.AssignCovidPass(request.CovidPass);
                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new DomainException("Profile is already in quarantine.");
            }
        }
    }
}
