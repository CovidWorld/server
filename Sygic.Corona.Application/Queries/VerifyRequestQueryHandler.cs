using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.Authorization;

namespace Sygic.Corona.Application.Queries
{
    public class VerifyRequestQueryHandler : IRequestHandler<VerifyRequestQuery, bool>
    {
        private readonly IRepository repository;
        private readonly ISignVerification verification;

        public VerifyRequestQueryHandler(ISignVerification verification, IRepository repository)
        {
            this.verification = verification;
            this.repository = repository;
        }

        public async Task<bool> Handle(VerifyRequestQuery request, CancellationToken cancellationToken)
        {
            if (HttpMethods.IsGet(request.HttpRequest.Method))
            {
                return await HandleRequestWithoutBody(request, cancellationToken);
            }
            else
            {
                return await HandleRequestWithBody(request, cancellationToken);
            }
        }

        private async Task<bool> HandleRequestWithoutBody(VerifyRequestQuery request, CancellationToken cancellationToken)
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

            var profile = await repository.GetProfileAsyncNt(request.VerifiedRequest.ProfileId, request.VerifiedRequest.DeviceId, cancellationToken);
            if (profile == null)
            {
                return false;
            }

            var profilePublicKey = profile.PublicKey;
            if (string.IsNullOrEmpty(profilePublicKey))
            {
                return false;
            }

            return profilePublicKey == requestPublicKey;
        }

        private async Task<bool> HandleRequestWithBody(VerifyRequestQuery request, CancellationToken cancellationToken)
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

            var profile = await repository.GetProfileAsyncNt(request.VerifiedRequest.ProfileId, request.VerifiedRequest.DeviceId, cancellationToken);
            if (profile == null)
            {
                return false;
            }

            var profilePublicKey = profile.PublicKey;
            if (string.IsNullOrEmpty(profilePublicKey))
            {
                return false;
            }

            return profilePublicKey == requestPublicKey;
        }
    }
}