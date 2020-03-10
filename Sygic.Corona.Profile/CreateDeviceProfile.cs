using System.IO;
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
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Profile
{
    public class CreateDeviceProfile
    {
        private readonly IMediator mediator;

        public CreateDeviceProfile(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("CreateDeviceProfile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateProfileRequest>(requestBody);
            
            var command = new CreateProfileCommand(data.DeviceId, data.PushToken, data.Locale, data.Latitude, data.Longitude, data.Accuracy, "");
            var result = await mediator.Send(command, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
