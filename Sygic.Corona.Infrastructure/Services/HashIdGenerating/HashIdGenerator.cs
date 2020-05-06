using HashidsNet;

namespace Sygic.Corona.Infrastructure.Services.HashIdGenerating
{
    public class HashIdGenerator : IHashIdGenerator
    {
        private readonly IHashids hashGenerator;

        public HashIdGenerator(IHashids hashGenerator)
        {
            this.hashGenerator = hashGenerator;
        }
        public string Generate(long id)
        {
            return hashGenerator.EncodeLong(id);
        }
    }
}
