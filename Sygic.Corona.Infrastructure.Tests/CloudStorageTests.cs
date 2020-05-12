using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.CloudStorage;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class CloudStorageTests
    {
        private ICloudStorageManager manager;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var configurationSection = configuration.GetSection("Values");

            var storage = CloudStorageAccount.Parse(configurationSection["CloudStorageConnectionString"]);
            manager = new CloudStorageManager(storage, "testcontainer");

        }

        [Test]
        public async Task Append()
        {
            var fileName = "test.csv";
            var testData = "1,2,abcd";

            if (await manager.ExistAsync(fileName, default))
            {
                var sb = new StringBuilder();
                sb.Append("\n");
                sb.Append(testData);
                await using var stream = await ProcessData(sb.ToString(), default);
                await manager.AppendAsync(fileName, stream, default);
            }
            else
            {
                await using var stream = await ProcessData(testData, default);
                await manager.CreateAsync(fileName, stream, default);
            }
        }

        private async Task<Stream> ProcessData(string dataToSerialize, CancellationToken cancellationToken)
        {
            var dataArray = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(dataToSerialize));
            var stream = new MemoryStream();
            await stream.WriteAsync(dataArray, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
