using FluentAssertions;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.AndroidAttestation;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class AndroidAttesttationTests : TestBase
    {
        [Test]
        public void VeriffyTest()
        {
            var service = new OfflineAttestation();

            var statement = configurationSection["testAttestationStatement"];
            var result = service.ParseAndVerify(statement);

            result.Should().NotBeNull();
        }
    }
}
