using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class UpdateQuarantineCommandHandler : AsyncRequestHandler<UpdateQuarantineCommand>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public UpdateQuarantineCommandHandler(IRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        protected override async Task Handle(UpdateQuarantineCommand request, CancellationToken cancellationToken)
        {
            var profiles = (await repository.GetProfilesByCovidPassAsync(request.CovidPass, cancellationToken)).ToList();

            if (!profiles.Any())
            {
                throw new DomainException($"No profiles found with {nameof(request.CovidPass)}: '{request.CovidPass}'");
            }
            
            foreach (var profile in profiles)
            {
                var notification = CreateQuarantineUpdatedNotification(profile, request.NotificationTitle, request.NotificationBody);

                profile.UpdateQuarantine(request.QuarantineStart, request.QuarantineEnd, request.BorderCrossedAt, request.QuarantineAddress);
                var command = new SendPushNotificationCommand(profile.Id, notification);
                await mediator.Send(command, cancellationToken);
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static Notification CreateQuarantineUpdatedNotification(Profile profile, string title, string body)
        {
            var message = new Notification
            {
                Priority = "high",
                Data = new Dictionary<string, object>
                {
                    {"type", "UPDATE_QUARANTINE_ALERT"},
                    {"start", profile.QuarantineBeginning},
                    {"end", profile.QuarantineEnd}
                },
                NotificationContent = new NotificationContent
                {
                    Title = title,
                    Body = body,
                    Sound = "default"
                },
                ContentAvailable = true
            };
            return message;
        }
    }
}