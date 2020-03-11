using System.Threading;
using System.Threading.Tasks;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Domain
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken);
        Task CreateContactAsync(Contact contact, CancellationToken cancellationToken);
        Task<Profile> GetProfileAsync(uint profileId, string deviceId, CancellationToken cancellationToken);
        Task<uint> GetLastIdAsync(CancellationToken cancellationToken);
        Task<bool> AlreadyCreatedAsync(string deviceId, CancellationToken cancellationToken);
    }
}
