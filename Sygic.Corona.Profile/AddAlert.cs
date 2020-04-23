using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Api
{
    public class AddAlert
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public AddAlert(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("AddAlert")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var qs = req.Query;

                if (!qs.ContainsKey("apiKey"))
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

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateAlertRequest>(requestBody);

                var command = new CreateAlertCommand(data.CovidPass, data.Content, data.WithPushNotification, data.PushSubject, data.PushBody);
                await mediator.Send(command, cancellationToken);
                return new OkResult();
            }
            catch (DomainException ex)
            {
                var errors = validation.ProcessErrors(ex);
                return new BadRequestObjectResult(errors);
            }
        }
    }
}
