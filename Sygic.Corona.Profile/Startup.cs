using System;
using System.Net.Http.Headers;
using System.Reflection;
using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sygic.Corona.Api;
using Sygic.Corona.Application.Behaviors;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.Authorization;
using Sygic.Corona.Infrastructure.Services.AutoNumberGenerating;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;
using Sygic.Corona.Infrastructure.Services.HashIdGenerating;
using Sygic.Corona.Infrastructure.Services.SmsMessaging;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sygic.Corona.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // inject your dependencies here
            builder.Services.AddLogging();

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
            builder.Services.AddSingleton(x => new TokenValidationParameters
            {
                ValidAudience = Environment.GetEnvironmentVariable("FirebaseProjectId"),
                ValidIssuer = $"https://securetoken.google.com/{Environment.GetEnvironmentVariable("FirebaseProjectId")}",
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            });
            builder.Services.AddHttpClient<IAuthService, FirebaseAuthService>(c =>
                {
                    c.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
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
                        c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MinvSmsUrl"));Environment.GetEnvironmentVariable("MinvSmsUrl");
                        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                    });
            }
            
            builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>(x => new TokenGenerator(Environment.GetEnvironmentVariable("MfaTokenGeneratorSecret")));
            builder.Services.AddSingleton<IDateTimeConvertService, DateTimeConvertService>();

            builder.Services.AddSingleton<IHashids>(x => new Hashids(
                Environment.GetEnvironmentVariable("MedicalIdHashSalt"), 
                int.Parse(Environment.GetEnvironmentVariable("MedicalIdHashMinValue")),
                Environment.GetEnvironmentVariable("MedicalIdHashAlphabet")));
            builder.Services.AddSingleton<IHashIdGenerator, HashIdGenerator>();

            builder.Services.AddSingleton<IAutoNumberGenerator, AutoNumberGenerator>(x => new AutoNumberGenerator(
                Environment.GetEnvironmentVariable("AutoNumberStorageConnection"),
                Environment.GetEnvironmentVariable("AutoNumberContainerName"),
                int.Parse(Environment.GetEnvironmentVariable("AutoNumberBatchSize")),
                int.Parse(Environment.GetEnvironmentVariable("AutoNumberMaxWriteAttempts"))));
        }
    }
}