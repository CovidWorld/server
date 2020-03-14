using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Contracts.Requests;
using Sygic.Corona.Domain.Common;

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateProfileRequest>(requestBody);
            
            var command = new CreateProfileCommand(data.DeviceId, data.PushToken, data.Locale, data.Latitude, data.Longitude, data.Accuracy);
            try
            {
                var result = await mediator.Send(command, cancellationToken);
                return new OkObjectResult(result);
            }
            catch (DomainException ex)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = "CreateDeviceProfile",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                if (ex.InnerException is ValidationException validationException)
                {
                    var propertyErrors = validationException.Errors.GroupBy(x => x.PropertyName);

                    foreach (var propertyError in propertyErrors)
                    {
                        problemDetails.Errors.Add(propertyError.Key, propertyError.Select(x => x.ErrorMessage).ToArray());
                    }
                }
                else
                {
                    problemDetails.Errors.Add("DomainValidations", new[] { ex.Message });
                }
                return new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
