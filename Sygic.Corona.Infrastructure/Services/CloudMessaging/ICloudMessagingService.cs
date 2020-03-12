using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public interface ICloudMessagingService
    {
        Task<CloudMessagingResponse> SendMessageToDevice(string registrationToken, object data, CancellationToken cancellationToken);
    }
}