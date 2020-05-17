using MediatR;
using Microsoft.AspNetCore.Http;
using Sygic.Corona.Contracts.Requests;
using System;

namespace Sygic.Corona.Application.Queries
{
    public class VerifyRequestWithoutPublicKeyValidationQuery : IRequest<bool>
    {
        public VerifiedRequestBase VerifiedRequest { get; }
        public HttpRequest HttpRequest { get; }

        public VerifyRequestWithoutPublicKeyValidationQuery(VerifiedRequestBase verifiedRequest, HttpRequest httpRequest)
        {
            VerifiedRequest = verifiedRequest ?? throw new ArgumentNullException(nameof(verifiedRequest));
            HttpRequest = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
        }
    }
}
