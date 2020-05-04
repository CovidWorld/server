using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Api
{
    public class AddContacts
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public AddContacts(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("AddContacts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, IDictionary<string, string> headers, CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestedVersion = ResolveVersion(headers);

            if (requestedVersion == "2.0")
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateContactsRequest>(requestBody);

                try
                {
                    var command = new AddContactsCommand(data.SourceDeviceId, data.SourceProfileId, data.Connections);
                    await mediator.Send(command, cancellationToken);
                    return new OkResult();

                }
                catch (DomainException ex)
                {
                    var errors = validation.ProcessErrors(ex);
                    return new BadRequestObjectResult(errors);
                }
            }

            return new NoContentResult();
        }

        private static string ResolveVersion(IDictionary<string, string> headers)
        {
            if (!headers.TryGetValue("Accept-Version", out string acceptHeader)) return null;
            if (acceptHeader.Equals("2.0", StringComparison.InvariantCultureIgnoreCase))
            {
                return "2.0";
            }

            return null;
        }
    }
}
