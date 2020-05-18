using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineQueryHandler : IRequestHandler<GetQuarantineQuery, GetQuarantineResponse>
    {
        private readonly IRepository repository;

        public GetQuarantineQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetQuarantineResponse> Handle(GetQuarantineQuery request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsyncNt(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }

            return new GetQuarantineResponse
            {
                CovidPass = profile.CovidPass,
                QuarantineStart = profile.QuarantineBeginning ?? default(DateTime?),
                QuarantineEnd = profile.QuarantineEnd ?? default(DateTime?),
                BorderCrossedAt = profile.BorderCrossedAt ?? default(DateTime?),
                Address = profile.QuarantineAddress != null ? new AddressResponse
                {
                    Latitude = profile.QuarantineAddress.Latitude,
                    Longitude = profile.QuarantineAddress.Longitude,
                    Country = profile.QuarantineAddress.CountryCode?.ToUpper(),
                    City = profile.QuarantineAddress.City,
                    ZipCode = profile.QuarantineAddress.ZipCode,
                    StreetName = profile.QuarantineAddress.StreetName,
                    StreetNumber = profile.QuarantineAddress.StreetNumber,
                } : null
            };
        }
    }
}