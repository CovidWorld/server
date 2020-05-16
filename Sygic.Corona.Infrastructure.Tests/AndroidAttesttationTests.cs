using FluentAssertions;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Parameters;
using Sygic.Corona.Infrastructure.Services.AndroidAttestation;
using Sygic.Corona.Infrastructure.Services.Authorization;
using System;
using System.Linq;

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
            var m = "{\"DeviceId\": \"7A7FA6EF-C86E-48C3-94C0-3B4FD588467B\",PofileId\": 17}";
            var service = new SignVerificationService();
            var message = configurationSection["testCipherMessage"];
            var key = configurationSection["testCipherKey"];
            var signature = configurationSection["testCipherSignature"];
            var result = service.Verify(m, key, signature);
        }

        [Test]
        public void SignatureChainTest()
        {
            var service = new SignVerificationService();

            var message = "super tajna sprava";
            var keys = service.GenerateKeyPair();
            var privateKey = keys.Private as ECPrivateKeyParameters;
            var publicKey = keys.Public as ECPublicKeyParameters;

            var signature = service.SignData(message, privateKey);

            var isVerified = service.Verify(message, publicKey, signature);
        }

        static string ToHex(byte[] data) => string.Concat(data.Select(x => x.ToString("x2")));

    }
}
