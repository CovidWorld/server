using System.IO;
using Microsoft.Extensions.Configuration;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class TestBase
    {
        protected readonly IConfigurationSection configurationSection;
        public TestBase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            configurationSection = configuration.GetSection("Values");
        }
    }
}
