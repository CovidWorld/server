using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.QuarantineApi
{
    public class GetPresenceCheck
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public GetPresenceCheck(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }
        
        [FunctionName("GetPresenceCheck")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "presencecheck/{covidPass}")]
            HttpRequest req,
            string covidPass,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                //
                if (!req.Query.TryGetValue("profileId", out var profileIdString))
                {
                    return new BadRequestErrorMessageResult("Missing query param: profileId");
                }

                if (!uint.TryParse(profileIdString, out var profileId))
                {
                    return new BadRequestErrorMessageResult($"Bad query param type: profileId. Cannot be cast to {profileId.GetType()}");
                }

                //
                if (!req.Query.TryGetValue("deviceId", out var deviceId))
                {
                    return new BadRequestErrorMessageResult("Missing query param: deviceId");
                }
                
                var data = new GetPresenceCheckRequest
                {
                    ProfileId = profileId,
                    DeviceId = deviceId,
                    CovidPass = covidPass
                };

                var verificationQuery = new VerifyRequestQuery(data, req);
                var isVerified = await mediator.Send(verificationQuery, cancellationToken);
                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }
                
                var query = new GetPresenceCheckQuery(data.ProfileId, data.DeviceId, data.CovidPass);
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