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
    public class SendMfaToken
    {
        private readonly IMediator mediator;

        public SendMfaToken(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("SendMfaToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<SendMfaTokenRequest>(requestBody);
            var command = new SendMfaTokenCommand(data.DeviceId, data.ProfileId, data.PhoneNumber);
            var result = await mediator.Send(command, cancellationToken);
            return new OkObjectResult(result);
        }
    }
}
