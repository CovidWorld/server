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
        private readonly IMediator mediator;
        private readonly IDateTimeConvertService timeConvertService;

        public NotifyAreaExitCommandHandler(IRepository repository, IMediator mediator, IDateTimeConvertService timeConvertService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.timeConvertService = timeConvertService ?? throw new ArgumentNullException(nameof(timeConvertService));
        }

        protected override async Task Handle(NotifyAreaExitCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            if (!profile.IsInQuarantine)
            {
                throw new DomainException("Profile is not in quarantine.");
            }

            var exit = new AreaExit(profile.Id, request.Severity, timeConvertService.UnixTimeStampToDateTime(request.RecordTimestamp));
            profile.AddAreaExit(exit);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var createPresenceCheckCommand = new CreatePresenceCheckCommand(profile.CovidPass, new TimeSpan(0), PresenceCheckStatus.LEFT);
            await mediator.Send(createPresenceCheckCommand, cancellationToken);
        }
    }
}
