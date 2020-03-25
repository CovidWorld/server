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

namespace Sygic.Corona.Api
{
    public class ReportLocation
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public ReportLocation(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("ReportLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ReportLocationRequest>(requestBody);
                var command = new ReportLocationCommand(data.ProfileId, data.DeviceId, data.Locations);

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
