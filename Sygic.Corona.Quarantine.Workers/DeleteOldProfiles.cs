using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MediatR;
using Sygic.Corona.Application.Commands;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Sygic.Corona.Quarantine.Workers
{
    public class DeleteOldProfiles
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public DeleteOldProfiles(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [Singleton]
        [FunctionName("DeleteOldProfiles")]
        public async Task Run(
            [TimerTrigger("%OldDataCleanCron%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            var command = new DeleteOldProfilesCommand(TimeSpan.Parse(configuration["OldDataCleanInterval"]));
            await mediator.Send(command, cancellationToken);
        }
    }
}
