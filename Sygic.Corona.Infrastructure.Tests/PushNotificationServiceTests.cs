using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class PushNotificationServiceTests
    {
        private HttpClient client;
        private FirebaseCloudMessagingService service;
        private readonly IConfigurationSection configurationSection;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var configurationSection = configuration.GetSection("Values");

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
                Data = new NotificationData
                {
                    Type = "MORNING_QUARANTINE_ALERT"
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
