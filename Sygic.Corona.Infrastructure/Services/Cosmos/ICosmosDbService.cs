using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.Cosmos
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<ProfileModel>> GetProfilesByPhoneNumberSearchTermAsync(string searchTerm, int limit, CancellationToken cancellationToken);
    }
}