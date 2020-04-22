using System;
using System.Net.Http.Headers;
using System.Reflection;
using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

namespace Sygic.Corona.Application.Tests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConfigurationSection Values => Configuration.GetSection("Values");
        
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // inject your dependencies here
            services.AddLogging();

            services.AddDbContext<CoronaContext>(o => o.UseCosmos(
                    Values["CosmosEndpoint"],
                    Values["CosmosAuthKey"],
                    Values["CosmosDatabase"]));

            services.AddScoped<ValidationProcessor>();
            //services.AddTransient(typeof(IValidator<CreateProfileCommand>), typeof(CreateProfileCommandValidator));
            //services.AddTransient(typeof(IValidator), typeof(AddContactsCommandValidator));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddScoped<IRepository, CoronaRepository>();
            services.AddMediatR(typeof(CreateProfileCommand).GetTypeInfo().Assembly);
            services.AddHttpClient<ICloudMessagingService, FirebaseCloudMessagingService>(c =>
            {
                c.BaseAddress = new Uri(Values["FirebaseUrl"]);
                c.DefaultRequestHeaders.Add("Authorization", $"key = {Values["FirebaseServerKey"]}");
                c.DefaultRequestHeaders.Add("Sender", $"id = {Values["FirebaseSenderId"]}");
            });
            services.AddSingleton(x => new TokenValidationParameters
            {
                ValidAudience = Values["FirebaseProjectId"],
                ValidIssuer = $"https://securetoken.google.com/{Values["FirebaseProjectId"]}",
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            });
            services.AddHttpClient<IAuthService, FirebaseAuthService>(c =>
                {
                    c.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
                });

            if (Environment.GetEnvironmentVariable("SmsProvider") == "Twilio")
            {
                services.AddSingleton<ISmsMessagingService, TwilioSmsMessagingService>(x => new TwilioSmsMessagingService(
                        Values["TwilioAccountSid"],
                        Values["TwilioAuthToken"],
                        Values["TwilioPhoneNumber"]));
            }
            else
            {
                var authenticationString = $"{Values["MinvSmsUserName"]}:{Values["MinvSmsPassword"]}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
                services.AddHttpClient<ISmsMessagingService, MinvSmsMessagingService>(c =>
                    {
                        c.BaseAddress = new Uri(Values["MinvSmsUrl"]);
                        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                    });
            }
            
            services.AddSingleton<ITokenGenerator, TokenGenerator>(x => new TokenGenerator(Values["MfaTokenGeneratorSecret"]));
            services.AddSingleton<IDateTimeConvertService, DateTimeConvertService>();

            services.AddSingleton<IHashids>(x => new Hashids(
                    Values["MedicalIdHashSalt"], 
                int.Parse(Values["MedicalIdHashMinValue"]),
                    Values["MedicalIdHashAlphabet"]));
            services.AddSingleton<IHashIdGenerator, HashIdGenerator>();

            services.AddSingleton<IAutoNumberGenerator, AutoNumberGenerator>(x => new AutoNumberGenerator(
                    Values["AutoNumberStorageConnection"],
                    Values["AutoNumberContainerName"],
                int.Parse(Values["AutoNumberBatchSize"]),
                int.Parse(Values["AutoNumberMaxWriteAttempts"])));

            return services.BuildServiceProvider();
        }
    }
}