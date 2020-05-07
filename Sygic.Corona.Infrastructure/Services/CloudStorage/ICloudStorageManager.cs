using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.CloudStorage
{
    public interface ICloudStorageManager
    {
        Task CreateAsync(string fileName, Stream data, CancellationToken cancellationToken);
        Task AppendAsync(string fileName, Stream data, CancellationToken cancellationToken);
        Task<bool> ExistAsync(string fileName, CancellationToken cancellationToken);
    }
}