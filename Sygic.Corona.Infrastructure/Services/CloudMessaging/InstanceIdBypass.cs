using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public class InstanceIdBypass : IInstanceIdService
    {
        public Task<InstanceInfo> GetInstanceInfoAsync(string pushToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(new InstanceInfo
            {
                Application = "test",
                ApplicationVersion = "test",
                AuthorizedEntity = "test",
                Platform = "test",
                Scope = "test"
            });
        }
    }
}
