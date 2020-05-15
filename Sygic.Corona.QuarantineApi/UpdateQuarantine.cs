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
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.QuarantineApi
{
    public class UpdateQuarantine
    {
        private readonly ISignVerification verification;
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public UpdateQuarantine(ISignVerification verification, IMediator mediator, ValidationProcessor validation)
        {
            this.verification = verification;
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("UpdateQuarantine")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "quarantine")]
            HttpRequest req,
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

                var data = JsonConvert.DeserializeObject<UpdateQuarantineRequest>(requestBody);
                var command = new UpdateQuarantineCommand(data.CovidPass, data.QuarantineStart, data.QuarantineEnd, data.BorderCrossedAt,
                    new Address(
                        data.QuarantineAddressLatitude, data.QuarantineAddressLongitude,
                        data.QuarantineAddressCountry, data.QuarantineAddressCity, data.QuarantineAddressCityZipCode,
                        data.QuarantineAddressStreetName, data.QuarantineAddressStreetNumber
                    )
                );

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