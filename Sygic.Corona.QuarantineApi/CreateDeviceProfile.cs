using System;
using System.IO;
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
using Sygic.Corona.Infrastructure.Services.ClientInfo;

namespace Sygic.Corona.QuarantineApi
{
    public class CreateDeviceProfile
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;
        private readonly IClientInfo clientInfoService;

        public CreateDeviceProfile(IMediator mediator, ValidationProcessor validation, IClientInfo clientInfo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.validation = validation ?? throw new ArgumentNullException(nameof(validation));
            this.clientInfoService = clientInfo ?? throw new ArgumentNullException(nameof(clientInfo));
        }

        [FunctionName("CreateDeviceProfile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "profile")] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {       
            try
            {
                var clientInfo = clientInfoService.Parse(req.Headers["User-Agent"]);
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();                
                var data = JsonConvert.DeserializeObject<CreateProfileRequest>(requestBody);

                var command = new CreateProfileCommand(data.DeviceId, data.PushToken, data.Locale, 
                    clientInfo.Name, clientInfo.Integrator, clientInfo.Version, clientInfo.OperationSystem);

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
