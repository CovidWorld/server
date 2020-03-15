using System.Linq;
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

            var locations = request.Locations.Select(x =>
                new Location(request.ProfileId, x.Latitude, x.Longitude, x.Accuracy, x.RecordDateUtc)).ToList();
            profile.AddLocations(locations);

            //foreach (var location in locations)
            //{
            //    await repository.CreateLocationAsync(location, cancellationToken);
            //}
            
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
