using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class MinvSmsMessagingServiceTests
    {
        private HttpClient client;
        private MinvSmsMessagingService service;
        private string testPhoneNumber;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var values = configuration.GetSection("Values");
            var url = values["MinvSmsUrl"];
            var userName = values["MinvSmsUserName"];
            var password = values["MinvSmsPassword"];

            testPhoneNumber = values["MinvSmsTestPhoneNumber"];

            client = new HttpClient() { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}")));

            service = new MinvSmsMessagingService(client);
        }


        [Test]
        public async Task Ping()
        {
            await service.PingAsync(CancellationToken.None);
        }

        [TestCase]
        public async Task SendSms()
        {
            await service.SendMessageAsync("Test message", testPhoneNumber, CancellationToken.None);
        }

        [TestCase]
        public async Task SendSmsError()
        {
            Assert.ThrowsAsync<DomainException>(async () =>
                    await service.SendMessageAsync("Test message", "+421111aaa", CancellationToken.None))
            ;
        }
    }
}