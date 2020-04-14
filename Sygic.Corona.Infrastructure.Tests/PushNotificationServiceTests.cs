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

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var values = configuration.GetSection("Values");

            client = new HttpClient { BaseAddress = new Uri(values["FirebaseUrl"]) };
            client.DefaultRequestHeaders.Add("Authorization", $"key = {values["FirebaseServerKey"]}");
            client.DefaultRequestHeaders.Add("Sender", $"id = {values["FirebaseSenderId"]}");
            service = new FirebaseCloudMessagingService(client);
        }

        [TestCase]
        public async Task SendNotificationToDeviceSuccessfully()
        {
            var token = "fg0mRwhzS96Lod-w7vxhdU:APA91bHqwmXZAGuCbiJyDI0Hr-XpgsgK_g0PVxBZUueJVGBu15elgIqSCUctbdXew8O0-08rqI6Tk74Aabcofd3_zkzM3JHTmzrAbd98T_RKxABfonVkedy5AiZ_uweiNFBsA_s293QE";


            var message = new Notification
            {
                Priority = "high",
                Data = new NotificationData
                {
                    Type = "MORNING_QUARANTINE_ALERT"
                },
                NotificationContent = new NotificationContent
                {
                    Title = "",
                    Body = "Test Message",
                    Sound = "default"
                },
                ContentAvailable = true
            };
            var result = await service.SendMessageToDevice(token, message, default);
        }
    }
}
