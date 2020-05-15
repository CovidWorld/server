using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Commands
{
    public class CreatePresenceCheckCommandHandler : AsyncRequestHandler<CreatePresenceCheckCommand>
    {
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public CreatePresenceCheckCommandHandler(IRepository repository, CoronaContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override Task Handle(CreatePresenceCheckCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}