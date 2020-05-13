using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class PushNotificationServiceTests : TestBase
    {
        private HttpClient client;
        private FirebaseCloudMessagingService service;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient { BaseAddress = new Uri(configurationSection["FirebaseUrl"]) };
            client.DefaultRequestHeaders.Add("Authorization", $"key = {configurationSection["FirebaseServerKey"]}");
            client.DefaultRequestHeaders.Add("Sender", $"id = {configurationSection["FirebaseSenderId"]}");
            service = new FirebaseCloudMessagingService(client);
        }

        [TestCase]
        public async Task SendNotificationToDeviceSuccessfully()
        {
            var token = configurationSection["testPushToken"];
            var message = new Notification
            {
                Priority = "high",
                Data = new Dictionary<string, object>
                {
                    { "type", "TEST" }
                },
                NotificationContent = new NotificationContent
                {
                    Title = " ---- TEST -----",
                    Body = "test",
                    Sound = "default"
                }
            };

            var result = await service.SendMessageToDevice(token, message, default);
        }
    }
}
