using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Infrastructure.Services.CloudStorage;

namespace Sygic.Corona.Application.Commands
{
    public class UploadExposureKeysCommandHandler : AsyncRequestHandler<UploadExposureKeysCommand>
    {
        private readonly ICloudStorageManager storageManager;

        public UploadExposureKeysCommandHandler(ICloudStorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        protected override async Task Handle(UploadExposureKeysCommand request, CancellationToken cancellationToken)
        {
            var fileName = $"TEK_{request.DaySpecification:yyyy}{request.DaySpecification:MM}{request.DaySpecification:dd}.csv";
            var builder = new StringBuilder();

            request.ExposureKeys.ForEach(x =>
            {
                builder.AppendLine(x.ToString());
            });
            await using var stream = await ConvertDataToStream(builder.ToString(), cancellationToken);

            if (await storageManager.ExistAsync(fileName, cancellationToken))
            {
                await storageManager.AppendAsync(fileName, stream, cancellationToken);
            }
            else
            {
                await storageManager.CreateAsync(fileName, stream, cancellationToken);
            }
        }

        private static async Task<Stream> ConvertDataToStream(string dataToSerialize, CancellationToken cancellationToken)
        {
            var dataArray = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(dataToSerialize));
            var stream = new MemoryStream();
            await stream.WriteAsync(dataArray, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
