using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Commands
{
    public class SendHeartbeatCommandHandler : AsyncRequestHandler<SendHeartbeatCommand>
    {
        private readonly CoronaContext context;
        private readonly IRepository repository;

        public SendHeartbeatCommandHandler(CoronaContext context, IRepository repository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(SendHeartbeatCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, request.CovidPass, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            if (!profile.IsInQuarantine)
            {
                throw new DomainException("Profile not in quarantine");
            }

            profile.UpdateLastPositionReportTime(DateTime.UtcNow);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}