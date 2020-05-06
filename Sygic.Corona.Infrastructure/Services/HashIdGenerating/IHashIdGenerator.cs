namespace Sygic.Corona.Infrastructure.Services.HashIdGenerating
{
    public interface IHashIdGenerator
    {
        string Generate(long id);
    }
}