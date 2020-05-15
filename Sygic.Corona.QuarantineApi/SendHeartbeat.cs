using System;
using System.IO;
using System.Linq;
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
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.QuarantineApi
{
    public class SendHeartbeat
    {
        private readonly ISignVerification verification;
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public SendHeartbeat(ISignVerification verification, IMediator mediator, ValidationProcessor validation)
        {
            this.verification = verification;
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("SendHeartbeat")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "heartbeat")] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string[] signatureHeaderParameters = req.Headers["X-Signature"].ToString().Split(':');
                if (signatureHeaderParameters.Length > 2)
                {
                    return new BadRequestResult();
                }
                var isVerified = verification.Verify(requestBody, signatureHeaderParameters.First(), signatureHeaderParameters.Last());

                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }

                var data = JsonConvert.DeserializeObject<HeartbeatRequest>(requestBody);
                var command = new SendHeartbeatCommand(data.DeviceId, data.ProfileId, data.CovidPass);
                
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