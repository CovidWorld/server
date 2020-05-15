using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using Sygic.Corona.Infrastructure.Services.Authorization;
using MediatR;
using System.Linq;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.QuarantineApi.Extensions;

namespace Sygic.Corona.QuarantineApi
{
    public class SendPushNonce
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public SendPushNonce(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("SendPushNonce")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pushnonce")] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var data = await req.DeserializeJsonBody<GetPushNonceRequest>();

                var verificationQuery = new VerifyRequestQuery(data, req);
                var isVerified = await mediator.Send(verificationQuery, cancellationToken);
                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }
                
                var generateNonceCommand = new GeneratePushNonceCommand(data.DeviceId, data.ProfileId, new TimeSpan(0, 10, 0));
                await mediator.Send(generateNonceCommand, cancellationToken);
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
