using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.QuarantineApi.Extensions;

namespace Sygic.Corona.QuarantineApi
{
    public class UpdatePresenceCheck
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public UpdatePresenceCheck(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }
        
        [FunctionName("UpdatePresenceCheck")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "presencecheck")]
            HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var data = await req.DeserializeJsonBody<UpdatePresenceCheckRequest>();

                var verificationQuery = new VerifyRequestQuery(data, req);
                var isVerified = await mediator.Send(verificationQuery, cancellationToken);
                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }
                
                var command = new UpdatePresenceCheckCommand(data.ProfileId, data.DeviceId, data.CovidPass, data.Status, data.Nonce);
                await mediator.Send(command, cancellationToken);

                return new OkObjectResult(new UpdatePresenceCheckResponse());
            }
            catch (DomainException ex)
            {
                var errors = validation.ProcessErrors(ex);
                return new BadRequestObjectResult(errors);
            }
        }
    }
}