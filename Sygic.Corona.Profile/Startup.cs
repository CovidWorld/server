﻿using System;
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

            builder.Services.AddTransient(typeof(IValidator<>), typeof(CreateProfileCommandValidator));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            builder.Services.AddScoped<IRepository, CoronaRepository>();
            builder.Services.AddMediatR(typeof(CreateProfileCommand).GetTypeInfo().Assembly);
        }
    }
}