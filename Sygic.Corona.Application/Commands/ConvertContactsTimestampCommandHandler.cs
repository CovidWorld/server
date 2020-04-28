using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Commands
{
    public class ConvertContactsTimestampCommandHandler : AsyncRequestHandler<ConvertContactsTimestampCommand>
    {
        private readonly CoronaContext context;
        private readonly IDateTimeConvertService convertService;

        public ConvertContactsTimestampCommandHandler(CoronaContext context, IDateTimeConvertService convertService)
        {
            this.context = context;
            this.convertService = convertService;
        }

        protected override async Task Handle(ConvertContactsTimestampCommand request, CancellationToken cancellationToken)
        {
            var contacts = await context.Contacts.ToListAsync(cancellationToken);

            foreach (var contact in contacts)
            {
                if (contact.Timestamp.HasValue)
                {
                    var creationDate = convertService.UnixTimeStampToDateTime(contact.Timestamp.Value).Date;
                    contact.SetCreationDate(creationDate);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
