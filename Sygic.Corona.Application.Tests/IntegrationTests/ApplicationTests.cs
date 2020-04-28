using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sygic.Corona.Application.Commands;
using Xunit;

namespace Sygic.Corona.Application.Tests.IntegrationTests
{
    public class ApplicationTests : TestBase
    {
        private readonly IServiceProvider provider;
        public ApplicationTests()
        {
            provider = CreateProvider();
        }
        
        [Fact]
        public async Task DeleteContactsWithLocation()
        {
            var command = new DeleteLocationsFromContactsCommand();
            var mediator = provider.GetService<IMediator>();
            await mediator.Send(command, default);
        }

        [Fact]
        public async Task ConvertContactsTimestampToDate()
        {
            var command = new ConvertContactsTimestampCommand();
            var mediator = provider.GetService<IMediator>();
            await mediator.Send(command, default);
        }

        [Fact]
        public async Task DeleteOldContacts()
        {
            var command = new DeleteOldContactsCommand(TimeSpan.FromDays(21));
            var mediator = provider.GetService<IMediator>();
            await mediator.Send(command, default);
        }
    }
}