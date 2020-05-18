using System;
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
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.timeConvertService = timeConvertService ?? throw new ArgumentNullException(nameof(timeConvertService));
        }

        protected override async Task Handle(NotifyAreaExitCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            if (!profile.ActiveQuarantine(DateTime.UtcNow))
            {
                throw new DomainException("Profile is not in quarantine.");
            }

            var exit = new AreaExit(profile.Id, request.Severity, timeConvertService.UnixTimeStampToDateTime(request.RecordTimestamp));
            profile.AddAreaExit(exit);

            var check = new PresenceCheck(profile.Id, now, now, PresenceCheckStatus.LEFT);
            profile.AddPresenceCheck(check);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
