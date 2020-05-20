using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Infrastructure;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldProfilesCommandHandler : AsyncRequestHandler<DeleteOldProfilesCommand>
    {
        private readonly CoronaContext context;
        private readonly ILogger<DeleteOldProfilesCommandHandler> log;
        public DeleteOldProfilesCommandHandler(CoronaContext context, ILogger<DeleteOldProfilesCommandHandler> log)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected override async Task Handle(DeleteOldProfilesCommand request, CancellationToken cancellationToken)
        {
            var treshold = DateTime.UtcNow.Add(-request.Interval);

            var expiredProfiles = await context.Profiles
                .Where(x => x.CreatedOn.HasValue && x.CreatedOn.Value <= treshold)
                .ToListAsync(cancellationToken);

            if (expiredProfiles.Any())
            {
                var builder = new StringBuilder();
                expiredProfiles.ForEach(x => builder.Append($"{x.CovidPass};"));
                log.LogInformation($"deleting profiles with covid passes {builder}");

                context.Profiles.RemoveRange(expiredProfiles);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
