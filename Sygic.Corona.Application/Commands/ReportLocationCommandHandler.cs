using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class ReportLocationCommandHandler : AsyncRequestHandler<ReportLocationCommand>
    {
        private readonly IRepository repository;

        public ReportLocationCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(ReportLocationCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            var location = new Location(request.Latitude, request.Longitude, request.Accuracy);
            profile.ReportPosition(location);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
