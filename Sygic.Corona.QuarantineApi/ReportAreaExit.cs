using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Infrastructure.Services.Authorization;
using MediatR;
using Sygic.Corona.Application.Validations;
using System.Threading;
using Sygic.Corona.Domain.Common;
using System.Linq;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Application.Commands;
using System;

namespace Sygic.Corona.QuarantineApi
{
    public class ReportAreaExit
    {
        private readonly ISignVerification verification;
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public ReportAreaExit(ISignVerification verification, IMediator mediator, ValidationProcessor validation)
        {
            this.verification = verification ?? throw new ArgumentNullException(nameof(verification));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        [FunctionName("ReportAreaExit")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "areaexit")] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string[] signatureHeaderParameters = req.Headers["X-Signature"].ToString().Split(':');
                if (signatureHeaderParameters.Length != 2)
                {
                    return new BadRequestResult();
                }
                var isVerified = verification.Verify(requestBody, signatureHeaderParameters.First(), signatureHeaderParameters.Last());

                if (!isVerified)
                {
                    return new UnauthorizedResult();
                }

                var data = JsonConvert.DeserializeObject<NotifyAreaExitRequest>(requestBody);

                var command = new NotifyAreaExitCommand(data.ProfileId,data.DeviceId, data.Severity, data.RecordTimestamp);
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
