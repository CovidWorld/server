using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldLocationsCommandHandler : AsyncRequestHandler<DeleteOldLocationsCommand>
    {
        private readonly IRepository repository;

        public DeleteOldLocationsCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(DeleteOldLocationsCommand request, CancellationToken cancellationToken)
        {
            var toInterval = DateTime.UtcNow.Add(-request.DeleteInterval);
            await repository.DeleteLocationsAsync(toInterval, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
