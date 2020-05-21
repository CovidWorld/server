using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
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

        protected override async Task Handle(CreatePresenceCheckCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var profiles = await repository.GetProfilesByCovidPassAsync(request.CovidPass, cancellationToken);

            if (!profiles.Any())
            {
                throw new DomainException($"No profiles found with {nameof(request.CovidPass)}: '{request.CovidPass}'");
            }

            foreach (var profile in profiles)
            {
                // meeting notes - always create presente check 

                //var activeCheck = await context.PresenceChecks.AsNoTracking()
                //    .Where(x => x.ProfileId == profile.Id && now < x.DeadLineCheck 
                //        && x.Status != PresenceCheckStatus.OK)
                //    .ToListAsync(cancellationToken);

                if (profile.ActiveQuarantine(now))
                {
                    var check = new PresenceCheck(profile.Id, now, now.Add(request.DeadLineTime), request.Status);
                    profile.AddPresenceCheck(check);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}