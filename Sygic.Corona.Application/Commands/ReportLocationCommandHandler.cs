using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Commands
{
    public class ReportLocationCommandHandler : AsyncRequestHandler<ReportLocationCommand>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService timeConvertService;

        public ReportLocationCommandHandler(IRepository repository, IDateTimeConvertService timeConvertService)
        {
            this.repository = repository;
            this.timeConvertService = timeConvertService;
        }

        protected override async Task Handle(ReportLocationCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            
            var locations = request.Locations.Select(x =>
            {
                var time = timeConvertService.UnixTimeStampToDateTime(x.RecordTimestamp);
                return new Location(request.ProfileId, x.Latitude, x.Longitude, x.Accuracy, time);
            }).ToList();
            profile.AddLocations(locations);

            //foreach (var location in locations)
            //{
            //    await repository.CreateLocationAsync(location, cancellationToken);
            //}
            
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
