using MediatR;
using Microsoft.AspNetCore.Http;
using Sygic.Corona.Infrastructure.Services.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Queries
{
    public class VerifyRequestWithoutPublicKeyValidationQueryHandler : IRequestHandler<VerifyRequestWithoutPublicKeyValidationQuery, bool>
    {
        private readonly ISignVerification verification;

        public VerifyRequestWithoutPublicKeyValidationQueryHandler(ISignVerification verification)
        {
            this.verification = verification ?? throw new ArgumentNullException(nameof(verification));
        }

        public async Task<bool> Handle(VerifyRequestWithoutPublicKeyValidationQuery request, CancellationToken cancellationToken)
        {
            if (HttpMethods.IsGet(request.HttpRequest.Method))
            {
                return HandleRequestWithoutBody(request, cancellationToken);
            }
            else
            {
                return await HandleRequestWithBody(request, cancellationToken);
            }
        }

        private bool HandleRequestWithoutBody(VerifyRequestWithoutPublicKeyValidationQuery request, CancellationToken cancellationToken)
        {
            var signatureHeaderParameters = request.HttpRequest.Headers["X-Signature"].ToString().Split(':');
            if (signatureHeaderParameters.Length != 2)
            {
                return false;
            }

            var requestPublicKey = signatureHeaderParameters.First();
            var isVerified = verification.Verify(request.HttpRequest.QueryString.Value, requestPublicKey, signatureHeaderParameters.Last());
            if (!isVerified)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> HandleRequestWithBody(VerifyRequestWithoutPublicKeyValidationQuery request, CancellationToken cancellationToken)
        {
            request.HttpRequest.Body.Seek(0, SeekOrigin.Begin);
            var requestBody = await new StreamReader(request.HttpRequest.Body).ReadToEndAsync();

            var signatureHeaderParameters = request.HttpRequest.Headers["X-Signature"].ToString().Split(':');
            if (signatureHeaderParameters.Length != 2)
            {
                return false;
            }

            var requestPublicKey = signatureHeaderParameters.First();
            var isVerified = verification.Verify(requestBody, requestPublicKey, signatureHeaderParameters.Last());
            if (!isVerified)
            {
                return false;
            }

            return true;
        }
    }
}
