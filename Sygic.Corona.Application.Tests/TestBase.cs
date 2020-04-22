using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sygic.Corona.Application.Tests
{
    public class TestBase
    {
        public TestServer CreateServer()
        {
            string path = Assembly.GetAssembly(typeof(TestBase))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("local.settings.json", optional: false, true)
                        .AddEnvironmentVariables();
                }).UseStartup<TestStartup>();

            var testServer = new TestServer(hostBuilder);
            return testServer;
        }

        public IServiceProvider CreateProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            
            var startup = new TestStartup(configuration);

            return startup.ConfigureServices(new ServiceCollection());
        }
    }
}