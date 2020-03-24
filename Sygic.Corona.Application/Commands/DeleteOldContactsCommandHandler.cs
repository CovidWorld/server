using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldContactsCommandHandler : AsyncRequestHandler<DeleteOldContactsCommand>
    {
        private readonly IRepository repository;
        private readonly IDateTimeConvertService dateTimeConvertor;

        public DeleteOldContactsCommandHandler(IRepository repository, IDateTimeConvertService dateTimeConvertor)
        {
            this.repository = repository;
            this.dateTimeConvertor = dateTimeConvertor;
        }
        protected override async Task Handle(DeleteOldContactsCommand request, CancellationToken cancellationToken)
        {
            var interval = DateTime.UtcNow.Add(-request.DeleteInterval);
            //1583929493
            //1585045152
            try
            {
                int intervalEpoch = (int)dateTimeConvertor.DateTimeToUnixTimestamp(interval);
                await repository.DeleteContactsAsync(intervalEpoch, cancellationToken);
                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }
    }
}
