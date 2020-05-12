using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteExposureKeysCommandHandler : AsyncRequestHandler<DeleteExposureKeysCommand>
    {
        private readonly CoronaContext context;

        public DeleteExposureKeysCommandHandler(CoronaContext context)
        {
            this.context = context;
        }

        protected override async Task Handle(DeleteExposureKeysCommand request, CancellationToken cancellationToken)
        {
            context.ExposureKeys.RemoveRange(request.ExposureKeys);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
