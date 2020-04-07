using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Repositories;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class RepositoryUnitTests
    {
        private readonly Mock<CoronaContext> contextMock;

        public RepositoryUnitTests()
        {
            contextMock = new Mock<CoronaContext>();
        }
        
        [Test]
        public async Task WhenMultipleProfilesWithSamePhoneNumberExist_AndOneSendLocation_NoOneIsSelected()
        {
            //Arrange

            var firstProfile = 
                new Profile(1, "testDevice1", "testPushToken1", "en-US", "TestAuthToken");
            firstProfile.BeginQuarantine(TimeSpan.FromDays(1));
            var secondProfile = 
                new Profile(2, "testDevice2", "testPushToken2", "en-US", "TestAuthToken");
            secondProfile.BeginQuarantine(TimeSpan.FromDays(1));
            secondProfile.AddLocations(new List<Location>
            {
                new Location(47.124566, 17.45874, 25)
            });
            
            var profiles = new List<Profile>
            {
                firstProfile,
                secondProfile
            };
            
            contextMock.Setup(x => x.Profiles).ReturnsDbSet(profiles);
            
            var repository = new CoronaRepository(contextMock.Object);
            
            //Act
            var inactiveProfiles = await repository
                .GetInactiveUsersInQuarantineAsync(DateTime.UtcNow.AddDays(-2),default(CancellationToken));
            
            //Assert
            inactiveProfiles.Should().BeEmpty();
        }
        
        [Test]
        public async Task WhenInactiveProfileExist_ItIsSelected()
        {
            //Arrange
            
            var firstProfile = 
                new Profile(1, "testDevice1", "testPushToken1", "en-US", "TestAuthToken");
            firstProfile.BeginQuarantine(TimeSpan.FromDays(1));
            
            var profiles = new List<Profile>
            {
                firstProfile
            };
            
            contextMock.Setup(x => x.Profiles).ReturnsDbSet(profiles);
            
            var repository = new CoronaRepository(contextMock.Object);
            
            //Act
            var inactiveProfiles = await repository
                .GetInactiveUsersInQuarantineAsync(DateTime.UtcNow.AddDays(-2),default(CancellationToken));
            
            //Assert
            inactiveProfiles.Should().HaveCount(1);
        }
        
        [Test]
        public async Task WhenMultipleProfilesWithSamePhoneNumberExist_AndNoOneSendLocation_OnlyOneProfileIsSelected()
        {
            //Arrange

            var firstProfile = 
                new Profile(1, "testDevice1", "testPushToken1", "en-US", "TestAuthToken");
            firstProfile.BeginQuarantine(TimeSpan.FromDays(1));
            var secondProfile = 
                new Profile(2, "testDevice2", "testPushToken2", "en-US", "TestAuthToken");
            secondProfile.BeginQuarantine(TimeSpan.FromDays(1));
            
            var profiles = new List<Profile>
            {
                firstProfile,
                secondProfile
            };
            
            contextMock.Setup(x => x.Profiles).ReturnsDbSet(profiles);
            
            var repository = new CoronaRepository(contextMock.Object);
            
            //Act
            var inactiveProfiles = await repository
                .GetInactiveUsersInQuarantineAsync(DateTime.UtcNow.AddDays(-2),default(CancellationToken));
            
            //Assert
            inactiveProfiles.Should().HaveCount(1);
        }
    }
}