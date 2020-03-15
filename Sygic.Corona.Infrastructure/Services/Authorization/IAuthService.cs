using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public interface IAuthService
    {
        Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken);
    }
}
