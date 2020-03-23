using System;
using System.Reflection;
using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sygic.Corona.Admin;
using Sygic.Corona.Application.Behaviors;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Application.Validations;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.Authorization;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;
using Sygic.Corona.Infrastructure.Services.HashIdGenerating;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sygic.Corona.Admin
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

            builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>(x => new TokenGenerator(Environment.GetEnvironmentVariable("MfaTokenGeneratorSecret")));
            builder.Services.AddSingleton<IDateTimeConvertService, DateTimeConvertService>();

            builder.Services.AddSingleton<IHashids>(x => new Hashids(
                Environment.GetEnvironmentVariable("ProfileHashIdSalt"),
                int.Parse(Environment.GetEnvironmentVariable("ProfileHashIdLength"))));
            builder.Services.AddSingleton<IHashIdGenerator, HashIdGenerator>();
        }
    }
}