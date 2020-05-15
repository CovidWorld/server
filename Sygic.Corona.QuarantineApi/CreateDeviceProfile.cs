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

namespace Sygic.Corona.QuarantineApi
{
    public class CreateDeviceProfile
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public CreateDeviceProfile(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        [FunctionName("CreateDeviceProfile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "profile")] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {       
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();                
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
