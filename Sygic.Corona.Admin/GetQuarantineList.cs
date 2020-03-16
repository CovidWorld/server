using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
            var authHeader = req.Headers.Authorization;
            
            if (authHeader == null)
            {
                log.LogWarning("Missing or invalid authorization header!");
                return new UnauthorizedResult();
            }
            
            bool isAuthorized = await authService.ValidateTokenAsync(authHeader.Parameter, cancellationToken);

            //covid-app-2809a
            if (!isAuthorized)
            {
                log.LogWarning("Unauthorized call.");
                return new UnauthorizedResult();
            }
            
            var command = new GetQuarantineListQuery();
            var result = await mediator.Send(command, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
