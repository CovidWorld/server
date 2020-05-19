using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineStatusQueryHandler : IRequestHandler<GetQuarantineStatusQuery, GetQuarantineStatusResponse>
    {
        private readonly IRepository repository;

        public GetQuarantineStatusQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetQuarantineStatusResponse> Handle(GetQuarantineStatusQuery request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);
            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            return new GetQuarantineStatusResponse
            {
                IsInQuarantine = profile.ActiveQuarantine(DateTime.UtcNow),
                QuarantineBeginning = profile.QuarantineBeginning,
                QuarantineEnd = profile.QuarantineEnd
            };
        }
    }
}
