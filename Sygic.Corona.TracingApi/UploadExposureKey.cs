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
using Sygic.Corona.Application.Queries;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.TracingApi
{
    public class UploadExposureKey
    {
        private readonly IMediator mediator;
        private readonly ValidationProcessor validation;

        public UploadExposureKey(IMediator mediator, ValidationProcessor validation)
        {
            this.mediator = mediator;
            this.validation = validation;
        }

        [FunctionName("UploadExposureKey")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UploadExposureKeyRequest>(requestBody);

            var addCommand = new AddExposureKeysCommand(data.ExposureKeys);
            await mediator.Send(addCommand, cancellationToken);

            var getCommand = new GetRotatedExposureKeysQuery(DateTime.Parse("2019-01-01"));
            var keys = await mediator.Send(getCommand, cancellationToken);

            var uploadCommand = new UploadExposureKeysCommand(keys, DateTime.UtcNow);
            await mediator.Send(uploadCommand, cancellationToken);

            var deleteCommand = new DeleteExposureKeysCommand(keys);
            await mediator.Send(deleteCommand, cancellationToken);

            return new OkResult();
        }
    }
}
