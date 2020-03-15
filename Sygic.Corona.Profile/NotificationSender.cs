using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.Profile
{
    public class NotificationSender
    {
        private readonly IMediator mediator;
        private readonly IAuthService authService;

        public NotificationSender(IMediator mediator, IAuthService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [FunctionName("NotificationSender")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            string authHeader = req.Headers["Authorization"].ToString().Split(' ')[1];
            bool isAuthorized = await authService.ValidateTokenAsync(authHeader, cancellationToken);
            //covid-app-2809a
            if (isAuthorized)
            {
                return new OkObjectResult(new {ok = "ok"});
            }
            else
            {
                return new UnauthorizedResult();
            }

            //log.LogInformation("C# HTTP trigger function processed a request.");
            //var data = new { test = 2 };
            //var command = new SendPushNotificationCommand("testDeviceId3", 4, data);
            //var result = await mediator.Send(command, cancellationToken);

            //return new OkObjectResult(result);
        }
    }
}
