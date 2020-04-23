using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Queries
{
    public class GetAlertsForProfileQueryHandler : IRequestHandler<GetAlertsForProfileQuery, IEnumerable<AlertResponse>>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService convertService;

        public GetAlertsForProfileQueryHandler(IRepository repository, IDateTimeConvertService convertService)
        {
            this.repository = repository;
            this.convertService = convertService;
        }

        public async Task<IEnumerable<AlertResponse>> Handle(GetAlertsForProfileQuery request, CancellationToken cancellationToken)
        {
            var alerts =
                 await repository.GetAlertsForProfileNt(request.ProfileId, request.DeviceId)
                     .ToListAsync(cancellationToken);

            return alerts.Select(x => new AlertResponse
            {
                Id = x.Id,
                Content = x.Content,
                Created = convertService.DateTimeToUnixTimestamp(x.CreatedOn)
            });
        }
    }
}
