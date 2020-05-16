using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.QuarantineApi
{
    public class GetAllPresenceChecks
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;
        private readonly IConfiguration configuration;

        public GetAllPresenceChecks(IMediator mediator, ValidationProcessor validation, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.validation = validation;
            this.configuration = configuration;
        }
        
        [FunctionName("GetAllPresenceChecks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "presencecheck")]
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
                
                if (!req.Query.TryGetValue("from", out var fromString))
                {
                    return new BadRequestErrorMessageResult("Missing query param: profileId");
                }

                if (!DateTime.TryParse(fromString, out var from))
                {
                    return new BadRequestErrorMessageResult($"Bad query param type: profileId. Cannot be cast to {from.GetType()}");
                }
                
                if (!req.Query.TryGetValue("to", out var toString))
                {
                    return new BadRequestErrorMessageResult("Missing query param: profileId");
                }

                if (!DateTime.TryParse(toString, out var to))
                {
                    return new BadRequestErrorMessageResult($"Bad query param type: profileId. Cannot be cast to {to.GetType()}");
                }
                
                var data = new GetAllPresenceChecksRequest
                {
                    From = from,
                    To = to
                };
                
                var query = new GetAllPresenceChecksQuery(data.From, data.To, TimeSpan.Parse(configuration["PresenceCheckOffset"]));
                var response = await mediator.Send(query, cancellationToken);

                return new OkObjectResult(response);
            }
            catch (DomainException ex)
            {
                var errors = validation.ProcessErrors(ex);
                return new BadRequestObjectResult(errors);
            }
        }
    }
}