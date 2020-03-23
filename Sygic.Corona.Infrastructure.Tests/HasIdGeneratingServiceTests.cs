using FluentAssertions;
using HashidsNet;
using NUnit.Framework;

namespace Sygic.Corona.Infrastructure.Tests
{
    public class HasIdGeneratingServiceTests
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
    }
}