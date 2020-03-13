using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Profile
{
    public class NotificationSender
    {
        private readonly IMediator mediator;

        public NotificationSender(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //[FunctionName("NotificationSender")]
        //public async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log, CancellationToken cancellationToken)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");
        //    var data = new { test = 2 };
        //    var command = new SendPushNotificationCommand("testDeviceId3", 4, data);
        //    var result = await mediator.Send(command, cancellationToken);

        //    return new OkObjectResult(result);
        //}
    }
}
