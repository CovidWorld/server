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
    public class GetQuarantineList
    {
        private readonly IMediator mediator;

        public GetQuarantineList(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("GetQuarantineList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            var command = new GetQuarantineListQuery();
            var result = await mediator.Send(command, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
