using AutoNumber;
using AutoNumber.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

namespace Sygic.Corona.Infrastructure.Services.AutoNumberGenerating
{
    public interface IAutoNumberGenerator
    {
        uint Generate(string scope);
    }

    public class AutoNumberGenerator : IAutoNumberGenerator
    {
        private readonly UniqueIdGenerator generator;

        public AutoNumberGenerator(string connectionString, string containerName, int batchSize, int maxWriteAttempts)
        {
            var blobStorageAccount = CloudStorageAccount.Parse(connectionString);

            var blobOptimisticDataStore = new BlobOptimisticDataStore(blobStorageAccount, containerName);
            var options = new AutoNumberOptions
            {
                BatchSize = batchSize,
                MaxWriteAttempts = maxWriteAttempts
            };
            IOptions<AutoNumberOptions> optionsProvider = new OptionsWrapper<AutoNumberOptions>(options);
            generator = new UniqueIdGenerator(blobOptimisticDataStore, optionsProvider);
        }

        public uint Generate(string scope)
        {
            long nextId = generator.NextId(scope);
            return (uint) nextId;
        }
    }
}
