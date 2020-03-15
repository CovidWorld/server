using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class NotifyAreaExitCommandHandler : AsyncRequestHandler<NotifyAreaExitCommand>
    {
        private readonly IRepository repository;

        public NotifyAreaExitCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(NotifyAreaExitCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            var exit = new AreaExit(request.Latitude, request.Longitude, request.Accuracy, request.RecordDateUtc);
            profile.AddAreaExit(exit);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
