using System;
using System.Net.Http.Headers;
using System.Reflection;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;
using Sygic.Corona.Workers;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sygic.Corona.Workers
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Inject dependencies here
            builder.Services.AddLogging();

            builder.Services.AddDbContext<CoronaContext>(o => o.UseCosmos(
                Environment.GetEnvironmentVariable("CosmosEndpoint"),
                Environment.GetEnvironmentVariable("CosmosAuthKey"),
                Environment.GetEnvironmentVariable("CosmosDatabase")));
            builder.Services.AddMediatR(typeof(CreateProfileCommand).GetTypeInfo().Assembly);
            builder.Services.AddScoped<IRepository, CoronaRepository>();

            builder.Services.AddHttpClient<ICloudMessagingService, FirebaseCloudMessagingService>(c =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FirebaseUrl"));
                c.DefaultRequestHeaders.Add("Authorization", $"key = {Environment.GetEnvironmentVariable("FirebaseServerKey")}");
                c.DefaultRequestHeaders.Add("Sender", $"id = {Environment.GetEnvironmentVariable("FirebaseSenderId")}");
            });

            if (Environment.GetEnvironmentVariable("SmsProvider") == "Twilio")
            {
                builder.Services.AddSingleton<ISmsMessagingService, TwilioSmsMessagingService>(x => new TwilioSmsMessagingService(
                    Environment.GetEnvironmentVariable("TwilioAccountSid"),
                    Environment.GetEnvironmentVariable("TwilioAuthToken"),
                    Environment.GetEnvironmentVariable("TwilioPhoneNumber")));
            }
            else
            {
                var authenticationString = $"{Environment.GetEnvironmentVariable("MinvSmsUserName")}:{Environment.GetEnvironmentVariable("MinvSmsPassword")}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
                builder.Services.AddHttpClient<ISmsMessagingService, MinvSmsMessagingService>(c =>
                {
                    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MinvSmsUrl")); Environment.GetEnvironmentVariable("MinvSmsUrl");
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                });
            }

            builder.Services.AddSingleton<IDateTimeConvertService, DateTimeConvertService>();
        }
    }
}
