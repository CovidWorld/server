using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Commands
{
    public class NotifyAreaExitCommandHandler : AsyncRequestHandler<NotifyAreaExitCommand>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService timeConvertService;

        public NotifyAreaExitCommandHandler(IRepository repository, IDateTimeConvertService timeConvertService)
        {
            this.repository = repository;
            this.timeConvertService = timeConvertService;
        }

        protected override async Task Handle(NotifyAreaExitCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            var exit = new AreaExit(request.Latitude, request.Longitude, request.Accuracy, timeConvertService.UnixTimeStampToDateTime(request.RecordTimestamp));
            profile.AddAreaExit(exit);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
