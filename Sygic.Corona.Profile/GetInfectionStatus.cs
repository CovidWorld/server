using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Api
{
    public class GetInfectionStatus
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public GetInfectionStatus(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("GetInfectionStatus")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                bool convertSuccess = uint.TryParse(req.Query["profileId"], out uint profileId);
                string deviceId = req.Query["deviceId"];

                if (convertSuccess == false)
                {
                    throw new DomainException("ProfileId is in wrong format.");
                }

                var query = new GetInfectionStatusQuery(profileId, deviceId);
                var result = await mediator.Send(query, cancellationToken);
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
