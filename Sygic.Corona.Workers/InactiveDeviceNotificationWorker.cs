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
    public class InactiveDeviceNotificationWorker
    {
        private readonly IMediator mediator;

        public InactiveDeviceNotificationWorker(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Singleton]
        [FunctionName("InactiveDeviceNotificationWorker")]
        public async Task Run([TimerTrigger("%InactiveDeviceCron%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var command = new GetInactiveProfilesQuery(DateTime.UtcNow, TimeSpan.Parse(Environment.GetEnvironmentVariable("CheckInterval")));
            var profiles = await mediator.Send(command, cancellationToken);

            foreach (var profile in profiles)
            {
                var notifyCommand = new SendNotificationToInactiveProfileCommand(profile, Environment.GetEnvironmentVariable("InactiveUserMessage"));
                await mediator.Send(notifyCommand, cancellationToken);
            }
        }
    }
}
