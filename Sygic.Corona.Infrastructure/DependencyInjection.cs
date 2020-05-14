using System;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Repositories;
using Sygic.Corona.Infrastructure.Services.AndroidAttestation;
using Sygic.Corona.Infrastructure.Services.Authorization;
using Sygic.Corona.Infrastructure.Services.ClientInfo;
using Sygic.Corona.Infrastructure.Services.CloudMessaging;
using Sygic.Corona.Infrastructure.Services.CloudStorage;
using Sygic.Corona.Infrastructure.Services.DateTimeConverting;
using Sygic.Corona.Infrastructure.Services.HashIdGenerating;
using Sygic.Corona.Infrastructure.Services.NonceGenerating;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

namespace Sygic.Corona.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoronaContext>(options =>
                options.UseSqlServer(
                    configuration["SqlDbConnection"],
                    b => b.MigrationsAssembly(typeof(CoronaContext).Assembly.FullName)));

            services.AddScoped<IRepository, CoronaRepository>();

            services.AddHttpClient<ICloudMessagingService, FirebaseCloudMessagingService>(c =>
            {
                c.BaseAddress = new Uri(configuration["FirebaseUrl"]);
                c.DefaultRequestHeaders.Add("Authorization", $"key = {configuration["FirebaseServerKey"]}");
                c.DefaultRequestHeaders.Add("Sender", $"id = {configuration["FirebaseSenderId"]}");
            });
            services.AddHttpClient<IInstanceIdService, FirebaseInstanceIdService>(c =>
            {
                c.BaseAddress = new Uri(configuration["FirebaseInstanceIdServiceUrl"]);
                c.DefaultRequestHeaders.Add("Authorization", $"key = {configuration["FirebaseServerKey"]}");
            });

            services.AddSingleton<IDateTimeConvertService, DateTimeConvertService>();

            services.AddSingleton<IHashids>(x => new Hashids(
                    configuration["MedicalIdHashSalt"],
                int.Parse(configuration["MedicalIdHashMinValue"]),
                    configuration["MedicalIdHashAlphabet"]));
            services.AddSingleton<IHashIdGenerator, HashIdGenerator>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>(x => new TokenGenerator(configuration["MfaTokenGeneratorSecret"]));

            services.AddSingleton(CloudStorageAccount.Parse(configuration["CloudStorageConnectionString"]));
            services.AddSingleton<ICloudStorageManager, CloudStorageManager>(x => 
                new CloudStorageManager(x.GetService<CloudStorageAccount>(), configuration["ExposureKeysContainerName"]));
            services.AddSingleton<IClientInfo, ClientInfoService>(sp => new ClientInfoService(configuration["UserAgentHeaderRegex"]));
            services.AddSingleton<INonceGenerator, NonceGenerator>();

            if (configuration["AZURE_FUNCTIONS_ENVIRONMENT"] == "Development")
            {
                services.AddSingleton<ISignVerification, SignVerificationBypass>();
                services.AddSingleton<IAndroidAttestation, OfflineAttestationBypass>();
            }
            else
            {
                services.AddSingleton<ISignVerification, SignVerificationService>();
                services.AddSingleton<IAndroidAttestation, OfflineAttestation>();
            }       

            return services;
        }
    }
}
