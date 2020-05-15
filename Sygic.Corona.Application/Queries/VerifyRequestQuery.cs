using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Queries
{
    public class VerifyRequestQuery : IRequest<bool>
    {
        public VerifyRequestQuery(VerifiedRequestBase verifiedRequest, HttpRequest httpRequest)
        {
            VerifiedRequest = verifiedRequest ?? throw new ArgumentNullException(nameof(verifiedRequest));
            HttpRequest = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
        }

        public VerifiedRequestBase VerifiedRequest { get; }
        public HttpRequest HttpRequest { get; }
    }
}