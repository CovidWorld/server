using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
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
            var profiles = await repository.GetProfilesByCovidPassAsync(request.CovidPass, cancellationToken);

            var profileList = profiles.ToList();

            var message = new Notification
            {
                //Priority = "high",
                //Data = new NotificationData
                //{
                //    Type = "UPDATE_QUARANTINE_ALERT"
                //},
                NotificationContent = new NotificationContent
                {
                    Title = "Covid19 ZostanZdravy",
                    Body = "Doba vasej karanteny sa zmenila, viac informacii najdete v aplikacii",
                    Sound = "default"
                },
                //ContentAvailable = true
            };


            if (profileList.Any())
            {
                foreach (var profile in profileList)
                {
                    profile.UpdateQuarantine(request.QuarantineStart, request.QuarantineEnd);
                    var command = new SendPushNotificationCommand(profile.Id, message);
                    await mediator.Send(command, cancellationToken);
                }
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}