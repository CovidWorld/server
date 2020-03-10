using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Infrastructure.Repositories
{
    public class CoronaRepository : IRepository
    {
        private readonly CoronaContext context;
        public IUnitOfWork UnitOfWork => context;

        public CoronaRepository(CoronaContext context)
        {
            this.context = context;
        }
        public async Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.Profiles.AddAsync(profile, cancellationToken);
        }

        public async Task<long> GetLastId(CancellationToken cancellationToken)
        {
            return await context.Profiles.OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
