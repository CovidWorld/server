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
                Application = "test.android.com",
                ApplicationVersion = "1.0.0",
                AuthorizedEntity = "test",
                Platform = Domain.Platform.Android,
                Scope = "test"
            });
        }
    }
}
