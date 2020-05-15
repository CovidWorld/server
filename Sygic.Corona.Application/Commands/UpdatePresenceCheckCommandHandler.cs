using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Commands
{
    public class UpdatePresenceCheckCommandHandler : AsyncRequestHandler<UpdatePresenceCheckCommand>
    {
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public UpdatePresenceCheckCommandHandler(IRepository repository, CoronaContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        protected override Task Handle(UpdatePresenceCheckCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}