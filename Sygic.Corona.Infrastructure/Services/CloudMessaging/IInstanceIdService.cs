using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public interface IInstanceIdService
    {
        Task<InstanceInfo> GetInstanceInfoAsync(string pushToken, CancellationToken cancellationToken);
    }
}