using System.Globalization;
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
    public class GetDeviceWithLocations
    {
        private readonly IMediator mediator;
        private readonly IAuthService authService;

        public GetDeviceWithLocations(
            IMediator mediator, 
            IAuthService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [FunctionName("GetDeviceWithLocations")]
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

            if (!isAuthorized)
            {
                log.LogWarning("Unauthorized call.");
                return new UnauthorizedResult();
            }

            var qs = req.RequestUri.ParseQueryString();

            var allKeys = qs.AllKeys;

            if (!allKeys.Contains("profileId"))
            {
                return new BadRequestErrorMessageResult($"Missing query param: profileId");
            }

            if (!allKeys.Contains("deviceId"))
            {
                return new BadRequestErrorMessageResult($"Missing query param: deviceId");
            }
            
            var profIdS = qs["profileId"];
            var devId = qs["deviceId"];

            if (!uint.TryParse(profIdS, NumberStyles.Any, CultureInfo.InvariantCulture, out var profId))
            {
                return new BadRequestErrorMessageResult($"Cannot parse profile id: {profIdS}");   
            }
            
            var command = new GetDeviceWithLocQuery(profId, devId);
            var result = await mediator.Send(command, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
