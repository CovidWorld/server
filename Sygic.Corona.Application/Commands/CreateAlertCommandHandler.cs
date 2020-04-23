using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Application.Commands
{
    public class CreateAlertCommandHandler : AsyncRequestHandler<CreateAlertCommand>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public CreateAlertCommandHandler(IRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        protected override async Task Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var profiles = await repository.GetProfilesByCovidPassAsync(request.CovidPass, cancellationToken);

            foreach (var profile in profiles)
            {
                profile.AddAlert(new Alert(profile.DeviceId, profile.Id, DateTime.UtcNow, request.Content));

                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                if (request.WithPushNotification.HasValue && request.WithPushNotification.Value)
                {
                    var message = new Notification
                    {
                        Priority = "high",
                        ContentAvailable = true,
                        NotificationContent = new NotificationContent
                        {
                            Title = request.PushSubject,
                            Body = request.PushBody,
                            Sound = "default"
                        }
                    };
                    var sendNotificationCommand = new SendPushNotificationCommand(profile.Id, message);
                    await mediator.Send(sendNotificationCommand, cancellationToken);
                }
            }
        }
    }
}
