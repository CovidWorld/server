using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Infrastructure.Services.Cosmos;

namespace Sygic.Corona.Application.Queries
{
    public class GetProfilesByPhoneNumberQueryHandler : IRequestHandler<GetProfilesByPhoneNumberQuery, IEnumerable<GetProfilesByPhoneNumberResponse>>
    {
        private readonly ICosmosDbService cosmosDbService;

        public GetProfilesByPhoneNumberQueryHandler(ICosmosDbService cosmosDbService)
        {
            this.cosmosDbService = cosmosDbService;
        }

        public async Task<IEnumerable<GetProfilesByPhoneNumberResponse>> Handle(GetProfilesByPhoneNumberQuery request, CancellationToken cancellationToken)
        {
            var profiles = await cosmosDbService.GetProfilesByPhoneNumberSearchTermAsync(request.SearchTherm, request.Limit, cancellationToken);
            
            var result = profiles.Select(x => new GetProfilesByPhoneNumberResponse
            {
                PhoneNumber = x.PhoneNumber,
                DeviceId = x.DeviceId,
                ProfileId = x.Id,
                LastPositionReportTime = x.LastPositionReportTime,
                IsInQuarantine = x.IsInQuarantine
            });

            return result;
        }
    }
}
