using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.Admin
{
    public class GetQuarantineList
    {
        private readonly IMediator mediator;
        private readonly IAuthService authService;

        public GetQuarantineList(
            IMediator mediator, 
            IAuthService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [FunctionName("GetQuarantineList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req,
            ILogger log, 
            CancellationToken cancellationToken)
        {
            var qs = req.RequestUri.ParseQueryString();

            var allKeys = qs.AllKeys;

            if (!allKeys.Contains("apiKey"))
            {
                return new BadRequestErrorMessageResult("Missing query param: apiKey");
            }

            var apiKey = qs["apiKey"];

            bool isAuthorized = apiKey == Environment.GetEnvironmentVariable("NcziApiKey");
            
            if (!isAuthorized)
            {
                log.LogWarning("Unauthorized call.");
                return new UnauthorizedResult();
            }

            var from = DateTime.Parse(qs["since"]);
            var command = new GetQuarantineListQuery(from);
            var result = await mediator.Send(command, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
