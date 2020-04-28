using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldContactsCommandHandler : AsyncRequestHandler<DeleteOldContactsCommand>
    {
        private readonly IRepository repository;

        public DeleteOldContactsCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(DeleteOldContactsCommand request, CancellationToken cancellationToken)
        {
            var interval = DateTime.UtcNow.Add(-request.DeleteInterval);

            await repository.DeleteContactsAsync(interval, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
