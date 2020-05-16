using FluentAssertions;
using NUnit.Framework;
using Sygic.Corona.Infrastructure.Services.AndroidAttestation;
using Sygic.Corona.Infrastructure.Services.Authorization;

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

        [Test]
        public void VeriffySignature()
        {
            var service = new SignVerificationService();
            var message = configurationSection["testCipherMessage"];
            var key = configurationSection["testCipherKey"];
            var signature = configurationSection["testCipherSignature"];
            service.Verify(message, key, signature);
        }
    }
}
