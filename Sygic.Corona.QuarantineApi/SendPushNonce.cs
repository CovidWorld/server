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
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.QuarantineApi
{
    public class SendPushNonce
    {
        private readonly ISignVerification verification;
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public SendPushNonce(ISignVerification verification, IMediator mediator, ValidationProcessor validation)
        {
            this.verification = verification;
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
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string[] signatureHeaderParameters = req.Headers["X-Siganture"].ToString().Split(':');
                if (signatureHeaderParameters.Length > 2)
                {
                    return new BadRequestResult();
                }
                var isVerified = verification.Verify(requestBody, signatureHeaderParameters.First(), signatureHeaderParameters.Last());

                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }

                var data = JsonConvert.DeserializeObject<GetPushNonceRequest>(requestBody);
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
