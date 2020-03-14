using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sygic.Corona.Application.Behaviors;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;
using Sygic.Corona.Profile;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sygic.Corona.Profile
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // inject your dependencies here
            builder.Services.AddDbContext<CoronaContext>(o => o.UseCosmos(
                Environment.GetEnvironmentVariable("CosmosEndpoint"),
                Environment.GetEnvironmentVariable("CosmosAuthKey"),
                Environment.GetEnvironmentVariable("CosmosDatabase")));

            builder.Services.AddScoped<ValidationProcessor>();
            builder.Services.AddTransient(typeof(IValidator<CreateProfileCommand>), typeof(CreateProfileCommandValidator));
            //builder.Services.AddTransient(typeof(IValidator), typeof(AddContactsCommandValidator));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            builder.Services.AddScoped<IRepository, CoronaRepository>();
            builder.Services.AddMediatR(typeof(CreateProfileCommand).GetTypeInfo().Assembly);
            builder.Services.AddHttpClient<ICloudMessagingService, FirebaseCloudMessagingService>(c =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FirebaseUrl"));
                c.DefaultRequestHeaders.Add("Authorization", $"key = {Environment.GetEnvironmentVariable("FirebaseServerKey")}");
                c.DefaultRequestHeaders.Add("Sender", $"id = {Environment.GetEnvironmentVariable("FirebaseSenderId")}");
            });
            builder.Services.AddSingleton<ISmsMessagingService, SmsMessagingService>(x => new SmsMessagingService(
                Environment.GetEnvironmentVariable("TwilioAccountSid"),
                Environment.GetEnvironmentVariable("TwilioAuthToken"),
                Environment.GetEnvironmentVariable("TwilioPhoneNumber")));
            builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>(x => new TokenGenerator(Environment.GetEnvironmentVariable("MfaTokenGeneratorSecret")));
        }
    }
}