using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Queries
{
    public class GetLastAlertForProfileQueryHandler : IRequestHandler<GetLastAlertForProfileQuery, AlertResponse>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService convertService;

        public GetLastAlertForProfileQueryHandler(IRepository repository, IDateTimeConvertService convertService)
        {
            this.repository = repository;
            this.convertService = convertService;
        }

        public async Task<AlertResponse> Handle(GetLastAlertForProfileQuery request, CancellationToken cancellationToken)
        {
            var lastAlert = await repository.GetAlertsForProfileNt(request.ProfileId, request.DeviceId)
                .FirstOrDefaultAsync(cancellationToken);

            var result = lastAlert != null ? new AlertResponse
            {
                Id = lastAlert.Id,
                Created = convertService.DateTimeToUnixTimestamp(lastAlert.CreatedOn),
                Content = lastAlert.Content
            } : new AlertResponse();

            return result;
        }
    }
}
