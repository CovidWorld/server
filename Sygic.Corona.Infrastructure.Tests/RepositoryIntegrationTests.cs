using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class RepositoryIntegrationTests : TestBase
    {
        private readonly CoronaRepository repository;
        private readonly FirebaseCloudMessagingService pushService;

        public RepositoryIntegrationTests()
        {
            var contextOption = new DbContextOptionsBuilder<CoronaContext>();
            contextOption.UseCosmos(
                configurationSection["CosmosEndpoint"],
                configurationSection["CosmosAuthKey"],
                configurationSection["CosmosDatabase"]);
            var context = new CoronaContext(contextOption.Options);
            repository = new CoronaRepository(context);

            var client = new HttpClient { BaseAddress = new Uri(configurationSection["FirebaseUrl"]) };
            client.DefaultRequestHeaders.Add("Authorization", $"key = {configurationSection["FirebaseServerKey"]}");
            client.DefaultRequestHeaders.Add("Sender", $"id = {configurationSection["FirebaseSenderId"]}");
            pushService = new FirebaseCloudMessagingService(client);
        }

        [Test]
        public async Task SendNotificationToAllProfilesWithSpecificCovidPass()
        {
            var covidPass = configurationSection["TestCovidPass"];
            var profiles = await repository.GetProfilesByCovidPassAsync(covidPass, default);

            var pushTokens = profiles.Select(x => x.PushToken).Where(x => !string.IsNullOrEmpty(x));

            var message = new Notification
            {
                Priority = "high",
                Data = new NotificationData
                {
                    Type = "MORNING_QUARANTINE_ALERT"
                },
                NotificationContent = new NotificationContent
                {
                    Title = "Toto je test ty huncut",
                    Body = "Test Message",
                    Sound = "default"
                },
                ContentAvailable = true
            };

            var results = new List<CloudMessagingResponse>();

            foreach (string pushToken in pushTokens)
            {
                var result = await pushService.SendMessageToDevice(pushToken, message, default);
                results.Add(result);
            }
        }
    }
}
