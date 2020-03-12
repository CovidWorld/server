using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Profile
{
    public class ConfirmInfection
    {
        private readonly IMediator mediator;

        public ConfirmInfection(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("ConfirmInfection")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ConfirmInfectionRequest>(requestBody);
            
            var command = new ConfirmInfectionCommand(data.DeviceId, data.ProfileId, data.MfaToken);
            await mediator.Send(command, cancellationToken);

            return new OkResult();
        }
    }
}
