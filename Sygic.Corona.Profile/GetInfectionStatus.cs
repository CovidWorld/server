using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Queries;

namespace Sygic.Corona.Profile
{
    public class GetInfectionStatus
    {
        private readonly IMediator mediator;

        public GetInfectionStatus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("GetInfectionStatus")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            bool convertSuccess = uint.TryParse(req.Query["profileId"], out uint profileId);
            string deviceId = req.Query["deviceId"];

            if (convertSuccess == false)
            {
                return new BadRequestObjectResult("ProfileId is in wrong format.");
            }

            var query = new GetInfectionStatusQuery(profileId, deviceId);
            var result = await mediator.Send(query, cancellationToken);
            return new OkObjectResult(result);
        }
    }
}
