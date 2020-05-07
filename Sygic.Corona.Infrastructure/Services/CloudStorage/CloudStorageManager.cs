using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sygic.Corona.Infrastructure.Services.CloudStorage
{
    public class CloudStorageManager : ICloudStorageManager
    {
        private readonly CloudStorageAccount storage;
        private readonly string containerName;

        public CloudStorageManager(CloudStorageAccount storage, string containerName)
        {
            this.storage = storage;
            this.containerName = containerName;
        }

        public async Task AppendAsync(string fileName, Stream data, CancellationToken cancellationToken)
        {
            var appendBlob = await GetBlobAsync(fileName);
            await appendBlob.AppendFromStreamAsync(data, null, null, null, cancellationToken);
        }

        public async Task CreateAsync(string fileName, Stream data, CancellationToken cancellationToken)
        {
            var appendBlob = await GetBlobAsync(fileName);
            await appendBlob.UploadFromStreamAsync(data, null, null, null, cancellationToken);
        }

        public async Task<bool> ExistAsync(string fileName, CancellationToken cancellationToken)
        {
            var appendBlob = await GetBlobAsync(fileName);
            return await appendBlob.ExistsAsync();
        }

        private async Task<CloudAppendBlob> GetBlobAsync(string fileName)
        {
            var blobClient = storage.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return container.GetAppendBlobReference(fileName);
        }
    }
}
