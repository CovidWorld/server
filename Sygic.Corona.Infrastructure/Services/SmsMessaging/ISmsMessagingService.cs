using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.SmsMessaging
{
    public interface ISmsMessagingService
    {
        Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken);
    }
}
