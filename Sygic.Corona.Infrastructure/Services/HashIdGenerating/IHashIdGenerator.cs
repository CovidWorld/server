namespace Sygic.Corona.Infrastructure.Services.HashIdGenerating
{
    public interface IHashIdGenerator
    {
        string Generate(uint id);
    }
}