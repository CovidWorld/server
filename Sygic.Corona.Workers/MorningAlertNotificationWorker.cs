using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Queries;

namespace Sygic.Corona.Workers
{
    public class MorningAlertNotificationWorker
    {
        private readonly IMediator mediator;

        public MorningAlertNotificationWorker(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Singleton]
        [FunctionName("MorningAlertNotificationWorker")]
        public async Task Run([TimerTrigger("%MorningAlertCron%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var command = new GetProfilesInQuarantineQuery();
            var profiles = await mediator.Send(command, cancellationToken);

            foreach (var profile in profiles)
            {
                var notificationCommand =
                    new SendMorningAlertNotificationCommand(
                        profile,
                        Environment.GetEnvironmentVariable("MorningAlertNotificationMessage")
                            ?? "Staying quarantined is safer for you and for the ones you love, stay safe.");
                await mediator.Send(notificationCommand, cancellationToken);
            }
        }
    }
}
