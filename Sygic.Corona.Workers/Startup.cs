using System;
using System.Reflection;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmsGate.Opis.Minv;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Repositories;
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

            if (Environment.GetEnvironmentVariable("SmsProvider") == "Twilio")
            {
                builder.Services.AddSingleton<ISmsMessagingService, TwilioSmsMessagingService>(x => new TwilioSmsMessagingService(
                    Environment.GetEnvironmentVariable("TwilioAccountSid"),
                    Environment.GetEnvironmentVariable("TwilioAuthToken"),
                    Environment.GetEnvironmentVariable("TwilioPhoneNumber")));
            }
            else
            {
                builder.Services.AddSingleton(x => new SmsExtClient(
                    Environment.GetEnvironmentVariable("MinvSmsUrl"),
                    TimeSpan.FromSeconds(30),
                    Environment.GetEnvironmentVariable("MinvSmsUserName"),
                    Environment.GetEnvironmentVariable("MinvSmsPassword")
                ));
                builder.Services.AddSingleton<ISmsMessagingService, MinvSmsMessagingService>();
            }
        }
    }
}
