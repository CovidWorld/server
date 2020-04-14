using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;

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
            var profiles = await repository.GetProfilesByCovidPassAsync(request.CovidPass, cancellationToken);

            var profileList = profiles.ToList();
            if (profileList.Any())
            {
                foreach (var profile in profileList)
                {
                    profile.UpdateQuarantine(request.QuarantineStart, request.QuarantineEnd);
                }
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}