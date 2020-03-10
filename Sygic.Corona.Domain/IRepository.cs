using System.Threading;
using System.Threading.Tasks;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Domain
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken);
        Task<long> GetLastId(CancellationToken cancellationToken);
    }
}
