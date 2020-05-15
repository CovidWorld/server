using FluentAssertions;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class InstanceIdServiceTests : TestBase
    {
        private HttpClient client;
        private FirebaseInstanceIdService service;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient { BaseAddress = new Uri(configurationSection["FirebaseInstanceIdServiceUrl"]) };
            client.DefaultRequestHeaders.Add("Authorization", $"key = {configurationSection["FirebaseServerKey"]}");
            service = new FirebaseInstanceIdService(client);
        }

        [Test]
        public async Task GetInstanceInfoSucessfully()
        {
            var token = configurationSection["testPushToken"];
            var info = await service.GetInstanceInfoAsync(token, default);

            info.Should().NotBeNull();
            info.Platform.Should().BeOfType(typeof(Domain.Platform));
        }
    }
}
