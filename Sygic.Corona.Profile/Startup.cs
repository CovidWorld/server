using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Sygic.Corona.Profile;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sygic.Corona.Profile
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // inject your dependencies here
        }
    }
}
