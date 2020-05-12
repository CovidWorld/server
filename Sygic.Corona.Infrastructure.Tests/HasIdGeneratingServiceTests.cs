using System.Text.RegularExpressions;
using FluentAssertions;
using HashidsNet;
using NUnit.Framework;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class HasIdGeneratingServiceTests : TestBase
    {
        private IHashids hashService;

        [SetUp]
        public void Setup()
        {
            hashService = new Hashids("this is a test salt", 5);
        }

        [Test]
        public void ComputeHash()
        {
            string hash = hashService.EncodeLong(1);

            hash.Should().Be("6axJN");
        }


        [TestCase("Navi(com.sygic.aura)/18.7.0(android 9)", "Navi", "com.sygic.aura", "18.7.0", "android")]
        [TestCase("SmartQuarantine(com.android.covid19zostanzdravy)/1.0.2(anr)", "SmartQuarantine", "com.android.covid19zostanzdravy", "1.0.2", "anr")]
        [TestCase("Tracing(com.android.covid19tracing)/1.5.0(ios)", "Tracing", "com.android.covid19tracing", "1.5.0", "ios")]
        public void RegexUserAgentHeaderTest(string header, string appName, string integrator, string appVersion, string operationSystem)
        {
            var userAgentRegex = new Regex(configurationSection["UserAgentHeaderRegex"]);

            var match = userAgentRegex.Match(header);

            match.Groups["app"].Value.Should().Be(appName);
            match.Groups["integrator"].Value.Should().Be(integrator);
            match.Groups["version"].Value.Should().Be(appVersion);
            match.Groups["os"].Value.Should().Be(operationSystem);
        }
    }
}