using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, GetContactsResponse>
    {
        private readonly IRepository repository;

        public GetContactsQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetContactsResponse> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await repository.GetContactsForProfileAsync(request.ProfileId, cancellationToken);
            var result = new GetContactsResponse();
            result.Contacts.AddRange(contacts.Select(x => new ContactResponse
            {
                ProfileId = x.ProfileId,
                SourceDeviceId = x.SourceDeviceId,
                SeenProfileId = x.SeenProfileId,
                Duration = x.Duration,
                CreatedOn = x.CreatedOn
            }));

            return result;
        }
    }
}
