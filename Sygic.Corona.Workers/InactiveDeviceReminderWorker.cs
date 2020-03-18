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
    public class InactiveDeviceReminderWorker
    {
        private readonly IMediator mediator;

        public InactiveDeviceReminderWorker(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Singleton]
        [FunctionName("InactiveDeviceReminderWorker")]
        public async Task Run([TimerTrigger("%InactiveDeviceReminderCron%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var query = new GetInactiveProfilesQuery(DateTime.UtcNow, TimeSpan.Parse(Environment.GetEnvironmentVariable("InactiveDeviceReminderCheckInterval")));
            var profiles = await mediator.Send(query, cancellationToken);

            var command = new SendReminderToInactiveDeviceCommand(profiles);
            await mediator.Send(command, cancellationToken);
        }
    }
}
