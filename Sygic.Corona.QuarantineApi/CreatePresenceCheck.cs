using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.QuarantineApi.Extensions;

namespace Sygic.Corona.QuarantineApi
{
    public class CreatePresenceCheck
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public CreatePresenceCheck(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }
        
        [FunctionName("CreatePresenceCheck")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "presencecheck")]
            HttpRequest req,
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
                
                var data = await req.DeserializeJsonBody<CreatePresenceCheckRequest>();

                var command = new CreatePresenceCheckCommand(data.CovidPass);
                await mediator.Send(command, cancellationToken);

                return new OkObjectResult(new CreatePresenceCheckResponse());
            }
            catch (DomainException ex)
            {
                var errors = validation.ProcessErrors(ex);
                return new BadRequestObjectResult(errors);
            }
        }
    }
}