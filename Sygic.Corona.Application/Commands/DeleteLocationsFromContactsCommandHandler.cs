using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteLocationsFromContactsCommandHandler : AsyncRequestHandler<DeleteLocationsFromContactsCommand>
    {
        private readonly CoronaContext context;

        public DeleteLocationsFromContactsCommandHandler(CoronaContext context)
        {
            this.context = context;
        }
        
        protected override async Task Handle(DeleteLocationsFromContactsCommand request, CancellationToken cancellationToken)
        {
            var contactsWithLocations = await context.Contacts
                .Where(x => x.Latitude != null || x.Longitude != null)
                .ToListAsync(cancellationToken);

            foreach (var contact in contactsWithLocations)
            {
                contact.ClearLocation();
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}