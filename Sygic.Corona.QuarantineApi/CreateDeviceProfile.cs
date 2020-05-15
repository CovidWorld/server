using System;
using System.IO;
using System.Linq;
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
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.QuarantineApi
{
    public class CreateDeviceProfile
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;
        private readonly ISignVerification verification;

        public CreateDeviceProfile(IMediator mediator, ValidationProcessor validation, ISignVerification verification)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.validation = validation ?? throw new ArgumentNullException(nameof(validation));
            this.verification = verification ?? throw new ArgumentNullException(nameof(verification));
        }

        [FunctionName("CreateDeviceProfile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "profile")] HttpRequest req,
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

                var data = JsonConvert.DeserializeObject<CreateProfileRequest>(requestBody);

                var command = new CreateProfileCommand(data.DeviceId, data.PushToken, data.Locale);

                var result = await mediator.Send(command, cancellationToken);
                return new OkObjectResult(result);
            }
            catch (DomainException ex)
            {
                var errors = validation.ProcessErrors(ex);
                return new BadRequestObjectResult(errors);
            }
        }
    }
}
