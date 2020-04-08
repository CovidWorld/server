using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class UpdateQuarantineCommandHandler : AsyncRequestHandler<UpdateQuarantineCommand>
    {
        private readonly IRepository repository;

        public UpdateQuarantineCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }
        
        protected override async Task Handle(UpdateQuarantineCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileByCovidPassAsync(request.CovidPass, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found");
            }
            
            profile.UpdateQuarantine(request.QuarantineStart, request.QuarantineEnd);

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}