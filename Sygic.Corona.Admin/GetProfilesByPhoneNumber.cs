using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using MediatR;
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.Admin
{
    public class GetProfilesByPhoneNumber
    {
        private readonly IMediator mediator;
        private readonly IAuthService authService;

        public GetProfilesByPhoneNumber(IMediator mediator, IAuthService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [FunctionName("GetProfilesByPhoneNumber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req,
            ILogger log, CancellationToken cancellationToken)
        {
            var authHeader = req.Headers.Authorization;

            if (authHeader == null)
            {
                log.LogWarning("Missing or invalid authorization header!");
                return new UnauthorizedResult();
            }

            bool isAuthorized = await authService.ValidateTokenAsync(authHeader.Parameter, cancellationToken);

            if (!isAuthorized)
            {
                log.LogWarning("Unauthorized call.");
                return new UnauthorizedResult();
            }

            var qs = req.RequestUri.ParseQueryString();

            var allKeys = qs.AllKeys;

            if (!allKeys.Contains("searchTherm"))
            {
                return new BadRequestErrorMessageResult("Missing query param: searchTherm");
            }

            if (!allKeys.Contains("limit"))
            {
                return new BadRequestErrorMessageResult("Missing query param: limit");
            }

            if (!int.TryParse(qs["limit"], out int limit))
            {
                return new BadRequestErrorMessageResult("Wrong query param: limit");
            }
            string searchTerm = qs["searchTherm"];


            var query = new GetProfilesByPhoneNumberQuery(searchTerm, limit);
            var result = await mediator.Send(query, cancellationToken);
           
            return new OkObjectResult(result);
        }
    }
}
