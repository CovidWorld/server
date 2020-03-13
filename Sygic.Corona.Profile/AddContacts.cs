using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Contracts.Requests;
using MediatR;

namespace Sygic.Corona.Profile
{
    public class AddContacts
    {
        private readonly IMediator mediator;

        public AddContacts(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("AddContacts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateContactsRequest>(requestBody);

            try
            {
                var command = new AddContactsCommand(data.SourceDeviceId, data.SourceProfileId, data.Connections);
                var result = await mediator.Send(command, cancellationToken);
                return new OkResult();

            }
            catch (Exception e)
            {
                log.LogWarning(e.Message);
                return new BadRequestResult();
            }
        }
    }
}
